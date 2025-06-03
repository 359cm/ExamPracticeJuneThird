using ExamPracticeJuneThird.Repository.Interfaces.User;
using ExamPracticeJuneThird.Services.DTOs.Authentication;
using ExamPracticeJuneThird.Services.Interfaces.Authentication;
using System.Data.SqlTypes;

namespace ExamPracticeJuneThird.Services.Implementations.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Username and password are required"
                };
            }

            var filter = new UserFilter { Username = new SqlString(request.Username) }; // fetch users by username
            var users = await _userRepository.RetrieveCollectionAsync(filter).ToListAsync(); // fetches users from the database where the username matches, converts the result into a List<User>
            var user = users.SingleOrDefault(); // Tries to find a single employee that matches

            if(user == null || user.Password != request.Password)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                Success = true,
                UserId = user.UserId,
                FullName = user.FullName
            };
        }
    }
}
