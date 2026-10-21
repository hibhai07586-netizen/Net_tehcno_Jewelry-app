using BCrypt.Net;
using Microsoft.Data.SqlClient;
// 1. Add the database and hashing namespaces
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Jewels_app
{
    public partial class Form1 : Form
    {
        // Define your connection string (Localhost, no password by default for XAMPP/MariaDB)
        private string connString = "server=localhost;uid=root;pwd=;database=luxe_jewels_db";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Call the database validation method
            if (ValidateLogin(username, password))
            {
                // Login successful
                home dashboardForm = new home();
                dashboardForm.Show();
                this.Hide();
            }
            else
            {
                // Login failed
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            bool isValid = false;
            string query = "SELECT Password FROM users WHERE UserName = @username LIMIT 1";

            // Call your central configuration file here:
            string connString = DatabaseConfig.GetConnectionString();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string hashedPasswordFromDb = result.ToString();

                            if (BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDb))
                            {
                                isValid = true;
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Database Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            return isValid;
        }


        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}
