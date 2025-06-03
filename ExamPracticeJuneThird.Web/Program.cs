using ExamPracticeJuneThird.Repository.Implementations.User;
using ExamPracticeJuneThird.Repository.Interfaces.User;
using ExamPracticeJuneThird.Services.Implementations.Authentication;
using ExamPracticeJuneThird.Services.Implementations.User;
using ExamPracticeJuneThird.Services.Interfaces.Authentication;
using ExamPracticeJuneThird.Services.Interfaces.User;
// other usings...

var builder = WebApplication.CreateBuilder(args);

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Initialize your ConnectionFactory static class
ExamPracticeJuneThird.Repository.ConnectionFactory.Initialize(connectionString);

// Register user services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// **Add this line to register the AuthenticationService implementation**
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add MVC services
builder.Services.AddControllersWithViews();

// Add session support (important for login session to work)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // **Add this so sessions are enabled**
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
