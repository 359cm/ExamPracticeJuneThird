using ExamPracticeJuneThird.Repository.Base;
using ExamPracticeJuneThird.Repository.Helpers;
using ExamPracticeJuneThird.Repository.Interfaces.User;
using Microsoft.Data.SqlClient;

namespace ExamPracticeJuneThird.Repository.Implementations.User
{
    public class UserRepository : BaseRepository<Models.User>, IUserRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId"; // This constant holds the name of the ID field (EmployeeId) used in the database
        protected override string GetTableName()
        {
            return "Users"; //Tells the base class which table this repository works with
        }
        protected override string[] GetColumns() => new[] // This defines the columns to select from the Users table in queries
        {
            IdDbFieldEnumeratorName,
            "Username",
            "Password",
            "FullName",
            "BirthDate"
        };
        protected override Models.User MapEntity(SqlDataReader reader) // maps a SqlDataReader row into an User object
        {
            return new Models.User
            {
                UserId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                Username = Convert.ToString(reader["Username"]),
                Password = Convert.ToString(reader["Password"]),
                FullName = Convert.ToString(reader["FullName"]),
                BirthDate = Convert.ToDateTime(reader["BirthDate"])
            };
        }
        public Task<int> CreateAsync(Models.User entity) // This method is supposed to insert a new User into the database, but it's currently not implemented. It should eventually override BaseRepository.CreateAsync() to define how a new employee is inserted.
        {
            throw new NotImplementedException();
        }
        public Task<Models.User> RetrieveAsync(int objectId) // fetch a user by ID
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Models.User> RetrieveCollectionAsync(UserFilter filter) // retrieves multiple employees, optionally filtering by Username
        {
            Filter commandFilter = new Filter();

            if (filter.Username is not null)
            {
                commandFilter.AddCondition("Username", filter.Username);
            }

            return base.RetrieveCollectionAsync(commandFilter);
        }
        public async Task<bool> UpdateAsync(int objectId, UserUpdate update) // updates the Password and FullName of a user by ID
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();

            UpdateCommand updateCommand = new UpdateCommand(
                connection,
                "Users",
                IdDbFieldEnumeratorName, objectId);

            updateCommand.AddSetClause("Password", update.Password);
            updateCommand.AddSetClause("FullName", update.FullName);

            return await updateCommand.ExecuteNonQueryAsync() > 0; // Ensures exactly one row is updated or throws
        }
        public Task<bool> DeleteAsync(int objectId) // This is supposed to delete a user by ID but isn’t implemented yet. You could override the BaseRepository.DeleteAsync() here similarly to the update method.
        {
            throw new NotImplementedException();
        }
    }
}
