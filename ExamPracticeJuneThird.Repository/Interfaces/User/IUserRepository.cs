using ExamPracticeJuneThird.Repository.Base;

namespace ExamPracticeJuneThird.Repository.Interfaces.User
{
    public interface IUserRepository : IBaseRepository<Models.User, UserFilter, UserUpdate>
    {
    }
}
