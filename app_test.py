from flask import Flask, render_template, request, redirect, url_for
import paramiko
import requests
import pymysql

app = Flask(__name__)

def run_predict_script(features):
    print("Running predict script...")
    hostname = ''
    username = 'ubuntu'
    key_path = 'C:/Users/Christopher.Munyau/.ssh/id_rsa'  # Updated path to your private key
    passphrase = ''  # Updated passphrase
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
        webhook_url = ''
        requests.post(webhook_url, json={"text": f"Patient Status: {prediction}"})
        return redirect(url_for('form'))
    return render_template('form.html')

if __name__ == '__main__':
    print("Starting Flask app...")
    app.run(debug=True)
