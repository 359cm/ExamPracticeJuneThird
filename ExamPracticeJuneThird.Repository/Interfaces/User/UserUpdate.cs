using System.Data.SqlTypes;

namespace ExamPracticeJuneThird.Repository.Interfaces.User
{
    public class UserUpdate
    {
        public SqlString? FullName { get; set; }
        public SqlString? Password { get; set; }
    }
}
