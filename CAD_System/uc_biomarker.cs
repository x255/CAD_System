using Guna.UI2.WinForms;
using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_System
{

    public partial class uc_biomarker : UserControl
    {
        public uc_biomarker()
        {
            InitializeComponent();
        }

        private void biomaker_script() {

            // Create Engine 
            var engine = Python.CreateEngine();

            // Set Python path to include the directory where numpy is installed
            engine.SetSearchPaths(new string[] { @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Lib\site-packages" , @"C:\Users\Christopher.Munyau\AppData\Local\anaconda3\libs" , @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\venv\Lib\site-packages\scipy\stats" });

            // Provide the script 

            var script = @"C:\Users\Christopher.Munyau\PycharmProjects\CAD_project\biomarkersinterfaceCMD.py";
            var source = engine.CreateScriptSourceFromFile(script);

            var argv = new List<string>();
            argv.Add("");
            argv.Add(guna2TextBox1.Text);
            argv.Add(guna2TextBox2.Text);
            argv.Add(guna2TextBox3.Text);

            engine.GetSysModule().SetVariable("arg",argv);


            // Output redirect

            var eIO = engine.Runtime.IO;

            //var errors = new MemoryStream();
            //eIO.SetErrorOutput(errors, Encoding.Default);

            var results = new MemoryStream();
            eIO.SetOutput(results, Encoding.Default);

            // Execute script

            var scope = engine.CreateScope();
            source.Execute(scope);

            // Display output

            string str(byte[]x) => Encoding.Default.GetString(x);

            this.Invoke((MethodInvoker)delegate
            {
                label5.Text = str(results.ToArray()); // Update Label5 with results
                //label6.Text = str(errors.ToArray()); //  Update Label6 with errors
            }
            );

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            biomaker_script();
        }
    }
}
