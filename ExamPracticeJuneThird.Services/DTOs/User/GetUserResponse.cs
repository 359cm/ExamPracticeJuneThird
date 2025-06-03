namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class GetUserResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
