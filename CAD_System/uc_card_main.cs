using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_System
{
    public partial class uc_card_main : UserControl
    {
        public uc_card_main()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                float age = float.Parse(guna2TextBox1.Text);
                float gender = float.Parse(guna2TextBox2.Text);
                float weight = float.Parse(guna2TextBox3.Text);
                float height = float.Parse(guna2TextBox4.Text);
                float ap_hi = float.Parse(guna2TextBox5.Text);
                float ap_lo = float.Parse(guna2TextBox6.Text);
                float cholesterol = float.Parse(guna2TextBox7.Text);
                float gluc = float.Parse(guna2TextBox8.Text);
                float smoke = float.Parse(guna2TextBox9.Text);
                float alco = float.Parse(guna2TextBox10.Text);
                float active = float.Parse(guna2TextBox11.Text);

                float[] features = { age, gender, weight, height, ap_hi, ap_lo, cholesterol, gluc, smoke, alco, active };

                string pythonScriptPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\load_card_model.py";
                string modelPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\dnn_model.h5";
                string scalerPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\scaler.joblib";
                string pythonExePath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Scripts\python.exe";
                string arguments = $"\"{pythonScriptPath}\" \"{modelPath}\" \"{scalerPath}\" {string.Join(" ", features)}";

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = pythonExePath;
                start.Arguments = arguments;
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd().Trim();
                        string error = process.StandardError.ReadToEnd().Trim();

                        if (!string.IsNullOrEmpty(error))
                        {
                            label5.Text = $"Python Error: {error}";
                            return;
                        }

                        // Debugging: Display the raw output from the Python script
                        label5.Text += $"\nRaw Python Output: {result}";

                        if (int.TryParse(result, out int prediction))
                        {
                            if (prediction == 0)
                            {
                                label5.Text = "Patient is CAD negative";
                            }
                            else
                            {
                                label5.Text = "Patient is CAD positive";
                            }
                        }
                        else
                        {
                            label5.Text = "Error: Invalid prediction result.";
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                label5.Text = $"Error: Invalid input format. Details: {ex.Message}";
            }
            catch (Exception ex)
            {
                label5.Text = $"Error: {ex.Message}";
            }
        }
    }
}
