using Bunifu.Framework.UI;
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
    public partial class TabloSayfaForm : Form
    {
        public TabloSayfaForm()
        {
            InitializeComponent();
        }

        private void buttonTablo1_Click(object sender, EventArgs e)
        {
            PersonelBilgileriSayfası personelBilgileriSayfası = new PersonelBilgileriSayfası();
            personelBilgileriSayfası.Show();
            this.Hide();
        }

        private void buttonDepartman_Click(object sender, EventArgs e)
        {
            DepartmanSayfası departmanSayfası = new DepartmanSayfası();
            departmanSayfası.Show();
            this.Hide();
        }

        private void buttonProje_Click(object sender, EventArgs e)
        {
            ProjeForm projeForm = new ProjeForm();
            projeForm.Show();
            this.Hide();
        }

        private void buttonSanal_Click(object sender, EventArgs e)
        {
            SanalTabloForm sanalTabloForm = new SanalTabloForm();
            sanalTabloForm.Show();
            this.Hide();

        }
    }
}
