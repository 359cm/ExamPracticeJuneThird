using ExamPracticeJuneThird.Repository.Helpers;
using Microsoft.Data.SqlClient;

namespace ExamPracticeJuneThird.Repository.Base
{
    public abstract class BaseRepository<TObj> // Abstract class: This means you cannot instantiate this class directly, it’s meant to be inherited.
    {
        protected abstract string GetTableName(); // Returns the name of the database table.
        protected abstract string[] GetColumns(); // Returns the list of columns in the table.
        protected virtual string SelectAllCommandText() // Constructs the basic SQL SELECT statement to retrieve all columns.
        {                                               // It's virtual meaning subclasses can override it if needed.
            var columns = string.Join(", ", GetColumns());
            return $"SELECT {columns} FROM {GetTableName()}";
        }
        protected abstract TObj MapEntity(SqlDataReader reader); // Converts a SqlDataReader row into a TObj instance.
        protected async Task<int> CreateAsync(TObj entity, string idDbFieldEnumeratorName = null) // Insert a new record into the database and return the newly created record’s ID.
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            var properties = typeof(TObj).GetProperties()
                .Where(p => p.Name != idDbFieldEnumeratorName)
                .ToList();

            string columns = string.Join(", ", properties.Select(p => p.Name));
            string parameters = string.Join(", ", properties.Select(p => "@" + p.Name)); // get all properties of TObj, excluding the ID field

            command.CommandText = $@"INSERT INTO {GetTableName()} ({columns}) 
                                VALUES ({parameters});
                                SELECT CAST(SCOPE_IDENTITY() as int)"; // Constructs an INSERT INTO SQL statement with columns and parameters.
                                                                       // uses SCOPE_IDENTITY() to get the inserted row’s ID
            foreach (var prop in properties)
            {
                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            return Convert.ToInt32(await command.ExecuteScalarAsync()); // Returns this ID as an int
        }
        protected async Task<TObj> RetrieveAsync(string idDbFieldName, int idDbFieldValue) // Retrieves one record by the given ID field name and value.
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText =
                $"{SelectAllCommandText()} " +
                $"WHERE {idDbFieldName} = @${idDbFieldName}"; // Builds a SQL SELECT statement filtering by the ID

            sqlCommand.Parameters.AddWithValue($"@${idDbFieldName}", idDbFieldValue); // Adds the ID parameter to the command.
            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync(); // uses a SqlDataReader to read the result

            if (reader.Read())
            {
                TObj result = MapEntity(reader); // Calls MapEntity(reader) to convert the row into a TObj

                if (reader.Read())
                {
                    throw new Exception("Multiple records found for the same ID.");
                }

                return result;
            }
            else
            {
                throw new Exception("No record found for the given ID.");
            }
        }
        protected async IAsyncEnumerable<TObj> RetrieveCollectionAsync(Filter filter) // Returns an asynchronous enumerable (IAsyncEnumerable) of objects matching the conditions specified in a Filter object
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText =
            @$"{SelectAllCommandText()} 
            WHERE 1 = 1";

            foreach (var condition in filter.Conditions)
            {
                sqlCommand.CommandText += $" AND {condition.Key} = @{condition.Key}";
                sqlCommand.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
            }

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                TObj employee = MapEntity(reader);
                yield return employee;
            }
        }
        protected async Task<bool> DeleteAsync(string idDbFieldName, int idDbFieldValue) // Deletes one record based on the given ID.
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();
            using SqlTransaction transaction = command.Connection.BeginTransaction();

            command.CommandText = $"DELETE FROM {GetTableName()} WHERE {idDbFieldName} = @{idDbFieldName}";
            command.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);
            command.Transaction = transaction;

            int rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected != 1)
            {
                throw new Exception($"Just one row should be deleted! Command aborted, because {rowsAffected} could have been deleted!");
            }

            transaction.Commit();
            return true; // Returns true indicating success
        }
    }
}
