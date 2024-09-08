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
    public partial class DepartmanSayfası : Form
    {
        private PersonelYönetimSınıfi personelYönetim;
        public DepartmanSayfası()
        {
            InitializeComponent();
            personelYönetim = new PersonelYönetimSınıfi();
        }

        private void buttonGöster_Click(object sender, EventArgs e)
        {

            try
            {
                // Departman adını al
                string departmentName = textboxDepartman.Text; // Departman adını buraya atayın veya bir textbox'tan alın

                // Departman adını kullanarak saklı yordamı çağırıp verileri al
                DataTable data = personelYönetim.CallStoredProcedureAndGetData(departmentName);

                // DataGridView'a verileri yükle
                dataGridView1.DataSource = data;
                // DataGridView'un görsel özelliklerini ayarla
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold); // Sütun başlıklarının fontunu ayarla
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10); // Hücrelerin fontunu ayarla

                dataGridView1.ColumnHeadersHeight = 30; // Sütun başlığı yüksekliğini ayarla
                dataGridView1.RowTemplate.Height = 25; // Satır yüksekliğini ayarla

                dataGridView1.EnableHeadersVisualStyles = false;

                // Sütunlar arasındaki boşluğu ayarla
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.Width = 150; // Sütun genişliğini ayarla (gerekirse her sütun için ayrı ayrı ayarlanabilir)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            string departman = textboxDepartman.Text;
            personelYönetim.AddDepartman(departman);
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {

            // DataGridView'de seçili satırın departman adını al
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string departmanAdi = dataGridView1.SelectedRows[0].Cells["departman_adı"].Value.ToString();

                // Departman adını kullanarak departman silme işlemini gerçekleştir
                personelYönetim.DeleteDepartman(departmanAdi);

               

            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonGüncelle_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satırın ID'sini al
                int departmanID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["departman_id"].Value);

                // Yeni departman adını TextBox'dan al
                string yeniDepartmanAdi = textboxDepartman.Text; // textboxYeniDepartman, yeni departman adını alacağınız TextBox

                // Boş olup olmadığını kontrol et
                if (!string.IsNullOrEmpty(yeniDepartmanAdi))
                {
                    // Sadece seçili departmanı güncelle
                    personelYönetim.UpdateDepartman(departmanID, yeniDepartmanAdi);

                    // Verileri tekrar yükle
                    // personelYönetim.LoadPersonelData(dataGridView1);
                }
                else
                {
                    MessageBox.Show("Yeni departman adını girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            TabloSayfaForm tablo_sayfasi = new TabloSayfaForm();
            tablo_sayfasi.Show();
            this.Hide();
        }

        private void buttonAra_Click(object sender, EventArgs e)
        {
            try
            {
                // Departman adını al
                string departmentName = textboxAra.Text.Trim(); // Trim kullanarak boşlukları kaldırın

                // Departman adını kullanarak filtreleme yap
                DataTable data = personelYönetim.GetDepartmanWithFilter(departmentName);

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
