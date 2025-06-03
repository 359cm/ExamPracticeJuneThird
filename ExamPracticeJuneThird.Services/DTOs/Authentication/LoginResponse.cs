namespace ExamPracticeJuneThird.Services.DTOs.Authentication
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
