using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamPracticeJuneThird.Web.Views.Home
{
    public class IndexModel : PageModel
    {
        public UserListViewModel UserList { get; set; }

        public void OnGet()
        {
            UserList = new UserListViewModel
            {
                Users = new List<UserViewModel>
                {
                    new UserViewModel { UserId = 1, Username = "jdoe", FullName = "John Doe", BirthDate = new DateTime(1990, 1, 1) },
                    new UserViewModel { UserId = 2, Username = "asmith", FullName = "Alice Smith", BirthDate = new DateTime(1985, 5, 23) }
                },
                TotalCount = 2
            };
        }
    }
}
