using System;
using Microsoft.Data.SqlClient;

class DatabaseTest
{
    static void Main()
    {
        string connectionString = "Server=tcp:az-sqldb-common.database.windows.net,1433;Initial Catalog=cdcsampledb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\"";

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection successful!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }
}
