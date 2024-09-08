using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ProjeTakip
{
    internal class PersonelYönetimSınıfi
    {
        //server url
        private string connectionString = "Server=.\\SQLEXPRESS;Database=personeldb;Integrated Security=True;";

    
        //Personel Ekleme
        public void AddPersonel(string ad, string soyad, decimal maas, int departmanId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM Departman WHERE departman_id = @departmanId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@departmanId", departmanId);
                    con.Open();
                    int departmanCount = (int)checkCmd.ExecuteScalar();
                    con.Close();

                    if (departmanCount == 0)
                    {
                        MessageBox.Show("Geçersiz departman ID. Lütfen geçerli bir departman ID giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string query = "INSERT INTO Personel (personel_ad, personel_soyad, personel_maas, departman_id) " +
                               "VALUES (@ad, @soyad, @maas, @departmanId)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@maas", maas);
                    cmd.Parameters.AddWithValue("@departmanId", departmanId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //Personel Güncelle
        public void UpdatePersonel(int id, string ad, string soyad, decimal maas, int departmanId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Personel SET personel_ad = @ad, personel_soyad = @soyad, personel_maas = @maas, departman_id = @departmanId " +
                               "WHERE personel_id = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@maas", maas);
                    cmd.Parameters.AddWithValue("@departmanId", departmanId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Personel Silme
        public void DeletePersonel(int personelId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Önce Proje tablosundaki ilgili projeleri sil
                        string deleteProjectsQuery = "DELETE FROM Proje WHERE personel_id = @personelId";
                        using (SqlCommand deleteProjectsCmd = new SqlCommand(deleteProjectsQuery, con, transaction))
                        {
                            deleteProjectsCmd.Parameters.AddWithValue("@personelId", personelId);
                            deleteProjectsCmd.ExecuteNonQuery();
                        }

                        // Ardından Personel tablosundaki kaydı sil
                        string deletePersonelQuery = "DELETE FROM Personel WHERE personel_id = @id";
                        using (SqlCommand deletePersonelCmd = new SqlCommand(deletePersonelQuery, con, transaction))
                        {
                            deletePersonelCmd.Parameters.AddWithValue("@id", personelId);
                            deletePersonelCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //İsim ile Personel Arama
        public void SearchPersonel(DataGridView dataGridView, string ad)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT p.personel_id, p.personel_ad, p.personel_soyad, p.personel_maas, d.departman_adı " +
                               "FROM Personel p " +
                               "INNER JOIN Departman d ON p.departman_id = d.departman_id " +
                               "WHERE p.personel_ad LIKE @ad";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@ad", "%" + ad);
                //da.SelectCommand.Parameters.AddWithValue("@soyad", "%" + soyad + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView.DataSource = dt;
            }
        }

        //Tüm personelleri gösterme
        public void LoadPersonelData(DataGridView dataGridView)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT p.personel_id, p.personel_ad, p.personel_soyad, p.personel_maas, d.departman_adı " +
                               "FROM Personel p " +
                               "INNER JOIN Departman d ON p.departman_id = d.departman_id";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                // DataGridView'un görsel özelliklerini ayarla
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold); // Sütun başlıklarının fontunu ayarla
                dataGridView.DefaultCellStyle.Font = new Font("Arial", 10); // Hücrelerin fontunu ayarla

                dataGridView.ColumnHeadersHeight = 30; // Sütun başlığı yüksekliğini ayarla
                dataGridView.RowTemplate.Height = 25; // Satır yüksekliğini ayarla

                dataGridView.EnableHeadersVisualStyles = false;

                // Sütunlar arasındaki boşluğu ayarla
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.Width = 150; // Sütun genişliğini ayarla (gerekirse her sütun için ayrı ayrı ayarlanabilir)
                }
                dataGridView.DataSource = dt;
            }
        }


        //Sanal tablo ile gösterme
        public void LoadPersonelProjeDataWithSanalTable(DataGridView dataGridView)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM vwPersonelProje";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // DataGridView stil ayarları
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold); // Sütun başlıklarının fontunu ayarla
                dataGridView.DefaultCellStyle.Font = new Font("Arial", 10); // Hücrelerin fontunu ayarla
                dataGridView.ColumnHeadersHeight = 30; // Sütun başlığı yüksekliğini ayarla
                dataGridView.RowTemplate.Height = 25; // Satır yüksekliğini ayarla
                dataGridView.EnableHeadersVisualStyles = false;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader; // Sütun genişliğini başlığa göre ayarla
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Sütun genişliğini içeriğe göre ayarla
                }



                dataGridView.DataSource = dt;
            }
        }



        //saklıyordam ile ekleme yapma
        public void AddDepartman(string departmanAdi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddDepartman", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@departmanAdi", departmanAdi); // @DepartmentName olarak değiştirildi

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //saklıyordam ile departman gösterme
        public DataTable CallStoredProcedureAndGetData(string departmanAdi)
        {
            DataTable dataTable = new DataTable();
            try
            {
                // SqlConnection nesnesi oluşturun
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SqlCommand nesnesi oluşturun ve sorguyu belirtin
                    string query = "SELECT departman_id,departman_adı FROM Departman";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // SqlDataAdapter ve DataTable nesnelerini kullanarak verileri alın
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        
                        adapter.Fill(dataTable);

                        // DataGridView'a verileri yükleyin
                        

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            return dataTable;
        }


        //saklıyordam ile silme
        public void DeleteDepartman(string departmanAdi)
        {
            try
            {
                // SqlConnection nesnesi oluşturun
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SqlCommand nesnesi oluşturun ve sorguyu belirtin
                    string query = "DELETE FROM Departman WHERE departman_adı = @departmanAdi";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekleyin
                        command.Parameters.AddWithValue("@departmanAdi", departmanAdi);

                        // Bağlantıyı açın ve sorguyu çalıştırın
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //saklıyordam ile güncelleme
        public void UpdateDepartman(int departmanID, string yeniDepartmanAdi)
        {
            try
            {
                // SqlConnection nesnesi oluşturun
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // SqlCommand nesnesi oluşturun ve sorguyu belirtin
                    string query = "UPDATE Departman SET departman_adı = @yeniDepartmanAdi WHERE departman_id = @departmanID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreleri ekleyin
                        command.Parameters.AddWithValue("@departmanID", departmanID);
                        command.Parameters.AddWithValue("@yeniDepartmanAdi", yeniDepartmanAdi);

                        // Bağlantıyı açın ve sorguyu çalıştırın
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //Departman Filtreleme
        public DataTable GetDepartmanWithFilter(string departmanAdi)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT departman_id, departman_adı FROM Departman WHERE departman_adı LIKE @departmanAdi";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@departmanAdi", "%" + departmanAdi + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        //Proje Filtreleme
        public DataTable GetProjenWithFilter(string departmanAdi)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT proje_adı FROM Proje WHERE proje_adı LIKE @departmanAdi";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@departmanAdi", "%" + departmanAdi + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        //Trigger
        //Göster
        public void getProje(DataGridView dataGridView)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT p.proje_id, p.proje_adı, pe.personel_ad, pe.personel_soyad " +
                               "FROM Proje p " +
                               "INNER JOIN Personel pe ON p.personel_id = pe.personel_id";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView.DataSource = dt;
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                dataGridView.EnableHeadersVisualStyles = false;
            }
        }
        //Güncelleme
        public void UpdateProje(int projeId, string yeniProjeAdi)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Proje SET proje_adı = @yeniProjeAdi WHERE proje_id = @projeId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@projeId", projeId);
                        cmd.Parameters.AddWithValue("@yeniProjeAdi", yeniProjeAdi);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}
