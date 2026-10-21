using System;
using System.Configuration;
// Change from Microsoft.Data.SqlClient to MySQL
using MySql.Data.MySqlClient;

namespace Jewels_app
{
    /// <summary>
    /// Centralized database configuration for all team members
    /// Keep this file updated and synced with the team
    /// </summary>
    public static class DatabaseConfig
    {
        // Your actual local MariaDB connection string
        public static string ConnectionString =
            "server=localhost;uid=root;pwd=;database=luxe_jewels_db";

        // Load from config file (App.config / Web.config)
        public static string GetConnectionString()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings["JewelsDB"]?.ConnectionString
                    ?? ConnectionString;
            }
            catch
            {
                return ConnectionString;
            }
        }

        // Validate connection
        public static bool TestConnection()
        {
            try
            {
                // Changed from SqlConnection to MySqlConnection
                using (var connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                return false;
            }
        }
    }
}
