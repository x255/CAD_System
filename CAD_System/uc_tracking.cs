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
    public partial class uc_tracking : UserControl
    {
        public uc_tracking()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imgPath = openFileDialog.FileName;
                string pythonScriptPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\predict_image.py";
                string modelPath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\model.hdf5";
                string pythonExePath = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Scripts\python.exe";
                string arguments = $"\"{pythonScriptPath}\" \"{modelPath}\" \"{imgPath}\"";

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

                        // Display the raw output from the Python script
                        label5.Text = result;
                        label5.AutoSize = false;
                        label5.MaximumSize = new Size(400, 0); // Adjust width as needed
                        label5.TextAlign = ContentAlignment.TopLeft;
                    }
                }
            }
        }
    }
}
