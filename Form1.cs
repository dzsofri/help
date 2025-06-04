// Szükséges névterek beemelése
using MySql.Data.MySqlClient; // MySQL kapcsolat
using System;
using System.Data;
using System.Windows.Forms;

namespace MySqlCrudApp
{
    /*
     * Az adatbázis és a tábla létrehozása MySQL-ben:
     * 
            CREATE DATABASE sampledb;
            USE sampledb;

            CREATE TABLE people (
                id INT AUTO_INCREMENT PRIMARY KEY,
                name VARCHAR(100),
                age INT
            );
     */

    public partial class MainForm : Form
    {
        // MySQL kapcsolat elérhetőségei
        string connectionString = "server=localhost;database=sampledb;uid=root;pwd=;";
        MySqlConnection connection;

        public MainForm()
        {
            InitializeComponent(); // Form komponenseinek inicializálása
            connection = new MySqlConnection(connectionString); // Kapcsolat példányosítása
            LoadData();     // Adatok betöltése a táblából a DataGridView-be
            ResetForm();    // Űrlap alaphelyzetbe állítása
        }

        // Adatok betöltése az adatbázisból a táblázatba
        private void LoadData()
        {
            try
            {
                connection.Open(); // Kapcsolat megnyitása
                string query = "SELECT * FROM people"; // SQL lekérdezés
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection); // Adapter létrehozása
                DataTable table = new DataTable(); // Üres DataTable
                adapter.Fill(table); // Feltöltés adatokkal
                dataGridView1.DataSource = table; // Megjelenítés a táblázatban
                connection.Close(); // Kapcsolat bezárása
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Hibakezelés
            }
        }

        // Form mezők kiürítése és gombok állapotának visszaállítása
        private void ResetForm()
        {
            txtId.Clear();
            txtName.Clear();
            txtAge.Clear();

            btnAdd.Enabled = true;     // Hozzáadás engedélyezve
            btnUpdate.Enabled = false; // Módosítás tiltva
            btnDelete.Enabled = false; // Törlés tiltva

            dataGridView1.ClearSelection(); // Sor kijelölés törlése
        }

        // Új adat hozzáadása
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO people (name, age) VALUES (@name, @age)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                LoadData();    // Frissítés
                ResetForm();   // Mezők ürítése
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Hibaüzenet
            }
        }

        // Kiválasztott adat módosítása
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE people SET name=@name, age=@age WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Kiválasztott adat törlése
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "DELETE FROM people WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtId.Text));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                LoadData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Táblázat sorára kattintva betölti az adatokat a mezőkbe
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ellenőrzés: nem fejléc-sorra kattintottunk-e
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtId.Text = row.Cells["id"].Value?.ToString();
                txtName.Text = row.Cells["name"].Value?.ToString();
                txtAge.Text = row.Cells["age"].Value?.ToString();

                // Gombok állapotának módosítása
                btnAdd.Enabled = false;     // Hozzáadás letiltva
                btnUpdate.Enabled = true;   // Módosítás engedélyezve
                btnDelete.Enabled = true;   // Törlés engedélyezve
            }
        }

        // Ha a form egy üres részére kattintunk → mezők törlése
        private void MainForm_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void MainForm_Click_1(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Összes adat törlése, megerősítő kérdéssel
        private void deleteAllbtn_Click(object sender, EventArgs e)
        {
            // Megerősítő ablak
            DialogResult result = MessageBox.Show(
                "Biztosan törölni szeretnél minden rekordot az adatbázisból?",
                "Megerősítés",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM people";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    LoadData();
                    ResetForm();
                    MessageBox.Show("Az összes rekord sikeresen törölve lett.", "Törlés kész", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("A törlés megszakítva.", "Művelet megszakítva", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
