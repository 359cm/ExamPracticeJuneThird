namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
