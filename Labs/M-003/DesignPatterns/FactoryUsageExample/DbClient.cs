using System.Data.Common;

namespace DesignPatterns.FactoryMethod
{
    /// <summary>
    /// Factory Usage Example
    /// </summary>
    public class DbClient
    {
        const string ConnectionStringSql = "Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;";
        const string ConnectionStringSqLite = "Data Source=Northwind.sqlite;Version=3;";

        public static void FactoryUsageExample()
        {
            Console.WriteLine("Factory usage example for DB connection");
            (DbProviderFactory factory, string connectionString) = CreateFactory();

            DbConnection? connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            Console.WriteLine(connection.State);

            DbCommand? command = factory.CreateCommand();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM Customers";

            object? rowCount = command.ExecuteScalar();
            Console.WriteLine("Employees: " + rowCount);

            connection.Close();
        }

        private static (DbProviderFactory, string) CreateFactory()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                return (Microsoft.Data.SqlClient.SqlClientFactory.Instance, ConnectionStringSql);
            }
            else
            {
                return (Microsoft.Data.Sqlite.SqliteFactory.Instance, ConnectionStringSqLite);
            }
        }
    }
}
