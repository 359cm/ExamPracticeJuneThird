using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace ExamPracticeJuneThird.Repository.Helpers
{
    public class UpdateCommand : IDisposable // This is a disposable class, meaning it manages unmanaged resources (like database connections) and should be cleaned up with Dispose() or using
    {
        private List<string> setClauses = new List<string>(); // setClauses: Holds parts of the SQL SET clause (e.g., Name = @Name)
        private SqlCommand sqlCommand; // The main command object used to run the SQL update
        private readonly string idDbFieldName;
        private readonly int idDbFieldValue; // The ID field and its value used to target the specific row for updating
        private SqlTransaction transaction; // Will be used to make the operation atomic and roll back on failure
        public UpdateCommand( // Initializes the sqlCommand object and sets the base command text to UPDATE [TableName]. No SET or WHERE clause yet — those are added later via methods
            SqlConnection sqlConnection,
            string tableName,
            string idDbFieldName,
            int idDbFieldValue)
        {
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"UPDATE {tableName}";

            this.idDbFieldName = idDbFieldName;
            this.idDbFieldValue = idDbFieldValue;
        }
        public void AddSetClause(string dbFieldName, INullable? dbFieldValue) // Adds a column-value pair to the SET clause of the SQL update
        {
            if (dbFieldValue is not null)
            {
                setClauses.Add($"[{dbFieldName}] = @{dbFieldName}"); // dbFieldName: Name of the column to update.
                sqlCommand.Parameters.AddWithValue($"@{dbFieldName}", dbFieldValue); // dbFieldValue: New value for that column. Must implement INullable (i.e., SQL-compatible types like SqlString, SqlInt32).
            }
        }
        public async Task<int> ExecuteNonQueryAsync() // Perform the Update
        {
            if (setClauses.Count == 0)
            {
                throw new Exception("No fields to update! You should pass at least one!");
            }

            sqlCommand.CommandText +=
            @$" SET {string.Join(", ", setClauses)}
            WHERE [{idDbFieldName}] = @{idDbFieldName}"; // UPDATE Users SET FirstName = @FirstName, Age = @Age WHERE Id = @Id

            sqlCommand.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);

            transaction = sqlCommand.Connection.BeginTransaction();
            sqlCommand.Transaction = transaction; // Add the ID parameter

            try
            {
                int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();

                if (rowsAffected != 1)
                {
                    transaction.Rollback();
                    throw new Exception($"Just one row should be updated! Command aborted, because {rowsAffected} could have been updated!");
                }

                transaction.Commit();
                return rowsAffected;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Dispose() // Ensures both the transaction and command are disposed when done
        {
            transaction?.Dispose(); // ?. avoids null reference exceptions
            sqlCommand?.Dispose();
        }
    }
}
