using Guna.UI2.WinForms;
using IronPython.Hosting;
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
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CAD_System
{

    public partial class uc_biomarker : UserControl
    {
        public uc_biomarker()
        {

        InitializeComponent();
    }



        private void biomaker_script_proc() {

            //// Create process info 

            //var psi = new ProcessStartInfo();
            //psi.FileName = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Scripts\python.exe";

            //// Privide script and arguments

            //var script = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\biomarkersinterfaceCMD.py";
            //var crp = (guna2TextBox1.Text);
            //var  trp = (guna2TextBox2.Text);
            //var hmo = (guna2TextBox3.Text);

            //psi.Arguments = $"\"{script}\" \"{crp}\" \"{trp}\" \"{hmo}\"";

            //// Process Configuration

            //psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;
            //psi.RedirectStandardOutput = true;
            //psi.RedirectStandardError = true;

            //// Execute process and get output

            //var errors = "";
            //var results = "";

            //using (var process = Process.Start(psi))
            //{
            //    errors = process.StandardError.ReadToEnd();
            //    results = process.StandardOutput.ReadToEnd();
            //}

            //// Dispaly output

            //this.Invoke((MethodInvoker)delegate
            //{
            //    label5.Text = results; // Update Label5 with results
            //    label6.Text = errors; // Update Label6 with errors
            //}
            //);


        }

        private void biomaker_script() {

            // Create Engine 
           // var engine = Python.CreateEngine();
           //


            // Set Python path to include the directory where numpy is installed
            //engine.SetSearchPaths(new string[] { @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Lib\site-packages" , @"C:\Users\Christopher.Munyau\AppData\Local\anaconda3\libs" , @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Lib\site-packages\scipy\stats" });

            // Provide the script 

            //var script = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\biomarkersinterfaceCMD.py";
            //var source = engine.CreateScriptSourceFromFile(script);

            //var argv = new List<string>();
           // argv.Add("");
           // argv.Add(guna2TextBox1.Text);
           // argv.Add(guna2TextBox2.Text);
           // argv.Add(guna2TextBox3.Text);

           // engine.GetSysModule().SetVariable("arg",argv);


            // Output redirect

            //var eIO = engine.Runtime.IO;

            //var errors = new MemoryStream();
            //eIO.SetErrorOutput(errors, Encoding.Default);

            //var results = new MemoryStream();
           // eIO.SetOutput(results, Encoding.Default);

            // Execute script

            //var scope = engine.CreateScope();
            //source.Execute(scope);

            // Display output

            //string str(byte[]x) => Encoding.Default.GetString(x);

            //this.Invoke((MethodInvoker)delegate
            //{
            //    label5.Text = str(results.ToArray()); // Update Label5 with results
                //label6.Text = str(errors.ToArray()); //  Update Label6 with errors
            //}
            //);

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //    //biomaker_script_proc();
            //    float crp = float.Parse(guna2TextBox1.Text);
            //    float troponin = float.Parse(guna2TextBox2.Text);
            //    float homocysteine = float.Parse(guna2TextBox3.Text);

            //    int crpRisk = crp < 0.2 ? 0 : 1;
            //    int troponinRisk = troponin < 14 ? 0 : 1;
            //    int homocysteineRisk = homocysteine < 15 ? 0 : 1;

            //    float[] features = { crpRisk, troponinRisk, homocysteineRisk };

            //    // Load your trained model
            //    var model = LoadModel(@"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\cad_model.joblib");

            //    // Predict
            //    var prediction = model.Predict(features);

            //    if (prediction[0] == 0)
            //    {
            //        label5.Text = "Patient is CAD negative";
            //    }
            //    else
            //    {
            //        label5.Text = "Patient is CAD positive";
            //    }
            //}

            //private dynamic LoadModel(string path)
            //{
            //    using (FileStream fs = new FileStream(path, FileMode.Open))
            //    {
            //        BinaryFormatter formatter = new BinaryFormatter();
            //        return formatter.Deserialize(fs);
            //    }

            try
            {
                // Debugging: Log the input values
                label5.Text = $"CRP: {guna2TextBox1.Text}, Troponin: {guna2TextBox2.Text}, Homocysteine: {guna2TextBox3.Text}";

                float crp = float.Parse(guna2TextBox1.Text);
                float troponin = float.Parse(guna2TextBox2.Text);
                float homocysteine = float.Parse(guna2TextBox3.Text);

                int crpRisk = crp < 0.2 ? 0 : 1;
                int troponinRisk = troponin < 14 ? 0 : 1;
                int homocysteineRisk = homocysteine < 15 ? 0 : 1;

                float[] features = { crpRisk, troponinRisk, homocysteineRisk };

                // Debugging: Log the risk values
                label5.Text += $"\nCRP Risk: {crpRisk}, Troponin Risk: {troponinRisk}, Homocysteine Risk: {homocysteineRisk}";

                string pythonScriptPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\load_model.py";
                string modelPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\cad_model.joblib";
                string pythonExePath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Scripts\python.exe";
                string arguments = $"\"{pythonScriptPath}\" \"{modelPath}\" {string.Join(" ", features)}";

                // Debugging: Log the arguments
                label5.Text += $"\nArguments: {arguments}";

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
