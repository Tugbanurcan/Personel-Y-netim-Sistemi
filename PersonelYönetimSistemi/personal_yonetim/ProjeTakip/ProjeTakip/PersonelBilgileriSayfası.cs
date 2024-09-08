using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjeTakip
{
    public partial class PersonelBilgileriSayfası : Form
    {
        private PersonelYönetimSınıfi personelYönetim;

        public PersonelBilgileriSayfası()
        {
            InitializeComponent();
            personelYönetim = new PersonelYönetimSınıfi();
        }

        private void PersonelBilgileriSayfası_Load(object sender, EventArgs e)
        {
            personelYönetim.LoadPersonelData(dataGridView1);
        }

        //Ekleme Butonu
        private void buttonEkle_Click(object sender, EventArgs e)
        {
            string ad = textboxAd.Text;
            string soyad = textboxSoyad.Text;
            decimal maas;
            int departmanId;

            if (decimal.TryParse(textboxMaas.Text, out maas) && int.TryParse(textboxDepartman.Text, out departmanId))
            {
                personelYönetim.AddPersonel(ad, soyad, maas, departmanId);
                personelYönetim.LoadPersonelData(dataGridView1);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir maaş ve departman ID girin.");
            }
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            // DataGridView'de seçili satırın ID'sini al
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["personel_id"].Value); // "ID" sütununun adı

                // ID değerini kullanarak personel silme işlemini gerçekleştir
                personelYönetim.DeletePersonel(id);
                personelYönetim.LoadPersonelData(dataGridView1);
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonGüncelle_Click(object sender, EventArgs e)
        {
            // DataGridView'de seçili satırın ID'sini al
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // ID ve diğer bilgileri al
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["personel_id"].Value);
                string ad = textboxAd.Text; 
                string soyad = textboxSoyad.Text; 
                decimal maas = Convert.ToDecimal(textboxMaas.Text); 
                int departmanId = Convert.ToInt32(textboxDepartman.Text); 

                // Personel bilgilerini güncelle
                personelYönetim.UpdatePersonel(id, ad, soyad, maas, departmanId);

                // DataGridView'i yenile
                personelYönetim.LoadPersonelData(dataGridView1); // DataGridView yenileme işlemi LoadPersonelData metoduna göre yapılmalıdır.
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonGöster_Click(object sender, EventArgs e)
        {
            personelYönetim.LoadPersonelData(dataGridView1);
        }

        private void buttonAra_Click(object sender, EventArgs e)
        {
            string ad = textboxAra.Text;
        
            personelYönetim.SearchPersonel(dataGridView1, ad);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            TabloSayfaForm tablo_sayfasi = new TabloSayfaForm();
            tablo_sayfasi.Show();
            this.Hide();
        }
    }
}
