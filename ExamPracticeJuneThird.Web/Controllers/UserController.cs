// Controllers/EmployeeController.cs
using ExamPracticeJuneThird.Services.Interfaces.User;
using ExamPracticeJuneThird.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

[Authorize] // Applied at the controller level, so all actions inside require the user to be authenticated.
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService; // Used to fetch employee data from the service layer (business logic).
    }
    public async Task<IActionResult> Index() // This is the default action for /Employee route.
    {
        var allUsers = await _userService.GetAllAsync(); // Fetches all employees
        var viewModel = new UserListViewModel
        {
            Users = allUsers.Users.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                FullName = u.FullName,
                BirthDate = u.BirthDate
            }).ToList(),
            TotalCount = allUsers.TotalCount
            // UpcomingBirthdays = upcomingBirthdays.Employees.Select(e => new EmployeeViewModel
            // {
            //    EmployeeId = e.EmployeeId,
            //    FullName = e.FullName,
            //    BirthDate = e.BirthDate,
            //    DaysToNextBirthday = e.DaysToNextBirthday
            //}).ToList()
        };

        return View(viewModel);
    }
}