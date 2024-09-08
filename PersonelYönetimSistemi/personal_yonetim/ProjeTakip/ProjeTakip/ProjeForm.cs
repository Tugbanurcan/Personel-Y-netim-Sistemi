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
    public partial class ProjeForm : Form
    {
        private PersonelYönetimSınıfi personelYönetim;
        public ProjeForm()
        {
            InitializeComponent();
            personelYönetim = new PersonelYönetimSınıfi();
        }

  

        private void buttonGöster_Click(object sender, EventArgs e)
        {
            personelYönetim.getProje(dataGridView1);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            TabloSayfaForm tablo_sayfasi = new TabloSayfaForm();
            tablo_sayfasi.Show();
            this.Hide();
        }

        private void buttonGüncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Proje ID ve yeni proje adını al
                int projeId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["proje_id"].Value);
                string yeniProjeAdi = textboxProje.Text;

                // Proje adını güncelle
                personelYönetim.UpdateProje(projeId, yeniProjeAdi);

                // Verileri yeniden yükle
                personelYönetim.getProje(dataGridView1);
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonAra_Click(object sender, EventArgs e)
        {
            try
            {
                // Departman adını al
                string departmentName = textboxAra.Text.Trim(); // Trim kullanarak boşlukları kaldırın

                // Departman adını kullanarak filtreleme yap
                DataTable data = personelYönetim.GetProjenWithFilter(departmentName);

                // DataGridView'a verileri yükle
                dataGridView1.DataSource = data;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.EnableHeadersVisualStyles = false;

                // Sütun genişliklerini başlıklara ve içeriğe göre otomatik olarak ayarla
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
