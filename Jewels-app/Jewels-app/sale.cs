using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Jewels_app
{
    public partial class Sale : UserControl
    {
        public Sale()
        {
            InitializeComponent();

            // Load database products automatically when control initializes
            LoadProductData();
        }

        private void Sale_Load(object sender, EventArgs e)
        {
            // Handled inside the constructor
        }

        private void Sale_Load_1(object sender, EventArgs e)
        {
            // Unused event handler stub
        }

        private void LoadProductData()
        {
            // Precise column definitions matching your database snapshot schema layout
            string query = "SELECT id, ProductID, ProductName, SKU, PurchasePrice, SellingPrice, StockQuantity, Description FROM products";

            // 💡 Pulls connection parameters dynamically from your centralized DatabaseConfig helper
            string connectionString = DatabaseConfig.GetConnectionString();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind data straight to your dgvProducts UI component
                        if (dgvProducts != null)
                        {
                            dgvProducts.DataSource = dt;
                        }
                        else
                        {
                            MessageBox.Show("UI Error: The DataGridView control 'dgvProducts' was not found on this panel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error: " + ex.Message, "Connection Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
