using Microsoft.AspNetCore.Mvc;
using ExamPracticeJuneThird.Services.Interfaces.User;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        // Check if user is logged in via session
        if (!HttpContext.Session.GetInt32("UserId").HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        // User is logged in, continue with existing logic
        var usersResult = await _userService.GetAllAsync();
        var usersDto = usersResult.Users;

        // Map UserInfo DTO to UserViewModel (adjust properties as needed)
        var usersViewModel = usersDto.Select(u => new UserViewModel
        {
            UserId = u.UserId,
            Username = u.Username,
            FullName = u.FullName,
            BirthDate = u.BirthDate
        }).ToList();

        return View(usersViewModel);
    }
}
