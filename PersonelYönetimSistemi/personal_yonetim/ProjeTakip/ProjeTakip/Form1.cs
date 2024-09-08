using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjeTakip
{
    public partial class LoginPageForm : Form
    {
        public LoginPageForm()
        {
            InitializeComponent();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if (bunifuMaterialTextbox1.Text == "admin" && bunifuMaterialTextbox2.Text == "admin123")
            {
                TabloSayfaForm tablo_sayfasi = new TabloSayfaForm();
                tablo_sayfasi.Show();
                this.Hide();
            }
        }

        private void LoginPageForm_Load(object sender, EventArgs e)
        {
            bunifuMaterialTextbox1.Focus();
            if (bunifuMaterialTextbox1.Text == "")
            {
                bunifuThinButton21.Enabled = false;
            }
            else
            {
                bunifuThinButton21.Enabled = true;
            }
        }

        private void bunifuMaterialTextbox1_OnValueChanged(object sender, EventArgs e)
        {
            bunifuThinButton21.Enabled = true;
        }

        private void bunifuMaterialTextbox2_OnValueChanged(object sender, EventArgs e)
        {

        }
    }
}
