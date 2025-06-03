using ExamPracticeJuneThird.Services.DTOs.Authentication;

namespace ExamPracticeJuneThird.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
