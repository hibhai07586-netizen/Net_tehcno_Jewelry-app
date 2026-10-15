using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Jewels_app
{
    /// <summary>
    /// Centralized database configuration for all team members
    /// Keep this file updated and synced with the team
    /// </summary>
    public static class DatabaseConfig
    {
        // Update this with your actual database connection string
        public static string ConnectionString = 
            "Server=YOUR_SERVER;Database=JewelsDB;User Id=sa;Password=YOUR_PASSWORD;";

        // Or load from config file
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
                using (var connection = new SqlConnection(GetConnectionString()))
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
