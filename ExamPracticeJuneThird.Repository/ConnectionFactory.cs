using Microsoft.Data.SqlClient;

namespace ExamPracticeJuneThird.Repository
{
    public static class ConnectionFactory
    {
        private static string? _connectionString; // holds the connection string used to connect to the SQL Server database
                                                  // The type string? means it is a nullable string (nullable reference type enabled). Initially, it can be null if not set.
                                                  // Being static means the value is shared across all usages of the ConnectionFactory class and persists for the app's lifetime.
        public static void Initialize(string connectionString) // initializes the connection factory by setting the _connectionString field
        {
            _connectionString = connectionString;
        }
        public static async Task<SqlConnection> CreateConnectionAsync() // creates and returns an open SQL connection
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
