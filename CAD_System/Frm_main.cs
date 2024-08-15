using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAD_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Home uc_home = new Home();
            addUserControl(uc_home);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            uc_card_main uc_card = new uc_card_main();
            addUserControl(uc_card);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panel_main.Controls.Clear();
            panel_main.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            uc_biomarker uc_bio  = new uc_biomarker();
            addUserControl(uc_bio);
            //panel10.BringToFront();
            //biomarkers frm = new biomarkers()
            //{
            //    Dock = DockStyle.Fill,
            //    TopLevel = false,
            //    TopMost = true

            //};
            //this.panel10.Controls.Add(frm);
            //frm.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Home uc_home = new Home();
            addUserControl(uc_home);

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            uc_tracking uc_track = new uc_tracking();
            addUserControl(uc_track);
        }
    }
}
