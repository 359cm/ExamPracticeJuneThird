using ExamPracticeJuneThird.Repository.Interfaces.User;
using ExamPracticeJuneThird.Services.DTOs.User;
using ExamPracticeJuneThird.Services.Helpers;
using ExamPracticeJuneThird.Services.Interfaces.User;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace ExamPracticeJuneThird.Services.Implementations.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository; // fetch/update user data from the database
        }
        public async Task<GetUserResponse> GetByIdAsync(int userId) // fetch a single employee by ID
        {
            var user = await _userRepository.RetrieveAsync(userId);

            return new GetUserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                BirthDate = user.BirthDate
            };
        }
        public async Task<GetAllUsersResponse> GetAllAsync()
        {
            var user = await _userRepository.RetrieveCollectionAsync(new UserFilter()).ToListAsync();

            var userInfos = user.Select(u =>
            {
                return new UserInfo
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    BirthDate = u.BirthDate
                };
            }).ToList();

            return new GetAllUsersResponse
            {
                Users = userInfos,
                TotalCount = userInfos.Count
            };
        }
        public async Task<UpdateUserResponse> UpdateFullNameAsync(UpdateFullNameRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NewFullName))
                    throw new ValidationException("Full name cannot be empty");

                var update = new UserUpdate
                {
                    FullName = new SqlString(request.NewFullName)
                };

                var success = await _userRepository.UpdateAsync(request.UserId, update);

                return new UpdateUserResponse
                {
                    Success = success,
                    UpdatedAt = DateTime.Now
                };
            }

            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    UpdatedAt = DateTime.Now
                };
            }
        }

        public async Task<UpdateUserResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NewPassword))
                    throw new ValidationException("Password cannot be empty");

                var hashedPassword = SecurityHelper.HashPassword(request.NewPassword);
                var update = new UserUpdate
                {
                    Password = new SqlString(hashedPassword)
                };

                var success = await _userRepository.UpdateAsync(request.UserId, update);

                return new UpdateUserResponse
                {
                    Success = success,
                    UpdatedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    UpdatedAt = DateTime.Now
                };
            }
        }
    }
}
