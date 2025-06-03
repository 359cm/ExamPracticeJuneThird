namespace ExamPracticeJuneThird.Services.DTOs.User
{
    public class GetAllUsersResponse
    {
        public List<UserInfo> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
