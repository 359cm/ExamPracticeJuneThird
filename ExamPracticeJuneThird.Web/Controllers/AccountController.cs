// Controllers/AccountController.cs
using ExamPracticeJuneThird.Services.DTOs.Authentication;
using ExamPracticeJuneThird.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;

    // Constructor to inject IAuthenticationService via dependency injection
    public AccountController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl }); // Displays the login form
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) // Checks if required fields like username/password are provided.
            return View(model);

        var result = await _authService.LoginAsync(new LoginRequest // Converts the view model into a DTO (LoginRequest) for service-level processing
        {                                                           // LoginAsync() checks the credentials and returns result.
            Username = model.Username,
            Password = model.Password
        });

        if (result.Success) // Saves user data in session for later use (e.g., access control).
        {
            HttpContext.Session.SetInt32("UserId", result.UserId.Value);
            HttpContext.Session.SetString("UserName", result.FullName);

            if (!string.IsNullOrEmpty(model.ReturnUrl))
                return Redirect(model.ReturnUrl); // Redirects to original page (ReturnUrl) or to homepage.

            return RedirectToAction("Index", "Home");
        }
        ViewData["ErrorMessage"] = result.ErrorMessage ?? "Invalid username or password";
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clears all session data (logs user out).
        return RedirectToAction("Login"); // Redirects back to the login screen.
    }
}
