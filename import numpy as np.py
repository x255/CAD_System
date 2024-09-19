import numpy as np
import sys
import os
import tensorflow as tf
import joblib
import warnings
import requests
from contextlib import redirect_stdout, redirect_stderr
import io

# Suppress TensorFlow logging messages
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'
os.environ['CUDA_VISIBLE_DEVICES'] = '-1'  # Disable CUDA
tf.get_logger().setLevel('ERROR')
tf.autograph.set_verbosity(3)

# Suppress all warnings
warnings.filterwarnings("ignore")

def load_and_predict(model_path, scaler_path, features):
    # Load the model and scaler
    model = tf.keras.models.load_model(model_path)
    scaler = joblib.load(scaler_path)

    # Scale the features
    features = np.array([features], dtype=np.float32)
    features = scaler.transform(features)

    # Make a prediction
    prediction = model.predict(features, verbose=0)  # Set verbose to 0 to suppress the progress bar
    binary_prediction = 1 if prediction[0][0] > 0.5 else 0
    return binary_prediction

if __name__ == "__main__":
    # Capture stdout and stderr
    f = io.StringIO()
    with redirect_stdout(f), redirect_stderr(f):
        # Get the model path, scaler path, and features from the command line arguments
        model_path = sys.argv[1]
        scaler_path = sys.argv[2]
        features = list(map(float, sys.argv[3:]))

        # Get the prediction result
        result = load_and_predict(model_path, scaler_path, features)

    # Filter the output to get only the prediction result
    output = f.getvalue().strip().split('\n')
    prediction_result = str(result)

    # Send the result to the Teams webhook
    webhook_url = 'https://liquidtelecommunications.webhook.office.com/webhookb2/c7765b1c-d569-4de3-8c31-57d7b34a7fb4@68792612-0f0e-46cb-b16a-fcb82fd80cb1/IncomingWebhook/40cbb88ca4144d509fa020fdc5d9198d/03e1c80e-6629-43d3-b282-a1ff742dab23'
    requests.post(webhook_url, json={"text": f"Patient Status: {prediction_result}"})

    # Print only the prediction result
    print(prediction_result)