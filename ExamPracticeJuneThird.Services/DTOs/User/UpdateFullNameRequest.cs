namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class UpdateFullNameRequest
    {
        public int UserId { get; set; }
        public string NewFullName { get; set; }
    }
}
