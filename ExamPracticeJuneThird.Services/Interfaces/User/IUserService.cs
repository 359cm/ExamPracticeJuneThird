using ExamPracticeJuneThird.Services.DTOs.User;

namespace ExamPracticeJuneThird.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<GetUserResponse> GetByIdAsync(int userId);
        Task<GetAllUsersResponse> GetAllAsync();
        Task<UpdateUserResponse> UpdateFullNameAsync(UpdateFullNameRequest request);
        Task<UpdateUserResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
    }
}
