namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; } 
        public string FullName { get; set; } 
        public DateTime BirthDate { get; internal set; }
    }
}
