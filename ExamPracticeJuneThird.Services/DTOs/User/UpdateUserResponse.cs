namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class UpdateUserResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
