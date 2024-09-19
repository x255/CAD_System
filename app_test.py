from flask import Flask, render_template, request, redirect, url_for
import paramiko
import requests
import pymysql

app = Flask(__name__)

def run_predict_script(features):
    print("Running predict script...")
    hostname = 'ec2-13-60-193-60.eu-north-1.compute.amazonaws.com'
    username = 'ubuntu'
    key_path = 'C:/Users/Christopher.Munyau/.ssh/id_rsa'  # Updated path to your private key
    passphrase = '#25@Dm1n25'  # Updated passphrase
    script_path = '/home/ubuntu/predict.py'
    model_path = '/home/ubuntu/dnn_model.h5'  # Replace with the actual model path
    scaler_path = '/home/ubuntu/scaler.joblib'  # Replace with the actual scaler path
    venv_python = '/home/ubuntu/myenv/bin/python3'  # Path to the Python interpreter in the virtual environment

    # Create the SSH client
    ssh = paramiko.SSHClient()
    ssh.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    private_key = paramiko.RSAKey.from_private_key_file(key_path, password=passphrase)
    ssh.connect(hostname, username=username, pkey=private_key)

    # Construct the command
    command = f"{venv_python} {script_path} {model_path} {scaler_path} {' '.join(map(str, features))}"

    # Execute the command
    stdin, stdout, stderr = ssh.exec_command(command)
    result = stdout.read().decode().strip()
    ssh.close()

    # Debugging: Print the raw result
    print(f"Raw result: {result}")

    # Extract the last line of the result, which should be the prediction
    prediction_result = result.split('\n')[-1]
    return prediction_result

def insert_into_db(features):
    print("Inserting into database...")
    connection = pymysql.connect(
        host='50.87.193.142',
        user='peppaimy_cad',
        password='&7PFzCQc@m^9de',
        database='peppaimy_cad_prd'
    )
    cursor = connection.cursor()
    sql = """
    INSERT INTO patient_data (age, gender, height, weight, ap_hi, ap_lo, cholesterol, gluc, smoke, alco, active)
    VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
    """
    cursor.execute(sql, features)
    connection.commit()
    cursor.close()
    connection.close()

@app.route('/')
def login():
    print("Rendering login page...")
    return render_template('login.html')

@app.route('/form', methods=['GET', 'POST'])
def form():
    print("Form route accessed...")
    if request.method == 'POST':
        print("Form submitted...")
        features = [
            request.form['age'],
            request.form['gender'],
            request.form['height'],
            request.form['weight'],
            request.form['ap_hi'],
            request.form['ap_lo'],
            request.form['cholesterol'],
            request.form['gluc'],
            request.form['smoke'],
            request.form['alco'],
            request.form['active']
        ]
        result = run_predict_script(features)
        insert_into_db(features)
        # Debugging: Print the parsed result
        print(f"Parsed result: {result}")

        # Interpret the result
        prediction = "CAD Positive" if result == "1" else "CAD Negative"
        # Send result to Teams webhook
        webhook_url = 'https://liquidtelecommunications.webhook.office.com/webhookb2/c7765b1c-d569-4de3-8c31-57d7b34a7fb4@68792612-0f0e-46cb-b16a-fcb82fd80cb1/IncomingWebhook/40cbb88ca4144d509fa020fdc5d9198d/03e1c80e-6629-43d3-b282-a1ff742dab23'
        requests.post(webhook_url, json={"text": f"Patient Status: {prediction}"})
        return redirect(url_for('form'))
    return render_template('form.html')

if __name__ == '__main__':
    print("Starting Flask app...")
    app.run(debug=True)
