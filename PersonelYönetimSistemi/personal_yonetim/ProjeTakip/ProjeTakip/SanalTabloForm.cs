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
    public partial class SanalTabloForm : Form
    {
        private PersonelYönetimSınıfi personelYönetim;
        public SanalTabloForm()
        {
            InitializeComponent();
            personelYönetim = new PersonelYönetimSınıfi();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            TabloSayfaForm tablo_sayfasi = new TabloSayfaForm();
            tablo_sayfasi.Show();
            this.Hide();
        }

        private void buttonGöster_Click(object sender, EventArgs e)
        {
            personelYönetim.LoadPersonelProjeDataWithSanalTable(dataGridView1);
        }
    }
}
