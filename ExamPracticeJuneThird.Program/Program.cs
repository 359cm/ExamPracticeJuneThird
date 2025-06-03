using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExamPracticeJuneThird.Repository;
using ExamPracticeJuneThird.Repository.Implementations.User;
using ExamPracticeJuneThird.Services.Implementations.Authentication;
using ExamPracticeJuneThird.Services.Implementations.User;
using ExamPracticeJuneThird.Repository.Interfaces.User;
using ExamPracticeJuneThird.Services.Interfaces.Authentication;
using ExamPracticeJuneThird.Services.Interfaces.User;
using ExamPracticeJuneThird.Services.DTOs.Authentication;

namespace ExamPracticeJuneThird.Program
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            ConnectionFactory.Initialize(connectionString); // Builds the configuration by reading the appsettings.json file in the current directory

            var services = new ServiceCollection();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var authService = serviceProvider.GetRequiredService<IAuthenticationService>();
                var userService = serviceProvider.GetRequiredService<IUserService>();

                Console.WriteLine("Login as first user (session creator):");
                Console.Write("Username: ");
                var username1 = Console.ReadLine();
                Console.Write("Password: ");
                var password1 = Console.ReadLine();

                var loginResult1 = await authService.LoginAsync(new LoginRequest
                {
                    Username = username1,
                    Password = password1
                });

                if (!loginResult1.Success)
                {
                    Console.WriteLine($"Login failed: {loginResult1.ErrorMessage}");
                    return;
                }

                Console.WriteLine($"Logged in as: {loginResult1.FullName}");

                var users = await userService.GetAllAsync();
                Console.WriteLine("\nCurrent users:");
                foreach (var us in users.Users)
                {
                    Console.WriteLine($"ID: {us.UserId}, Name: {us.FullName}");
                }

                Console.WriteLine("\nLogin as second user:");
                Console.Write("Username: ");
                var username2 = Console.ReadLine();
                Console.Write("Password: ");
                var password2 = Console.ReadLine();

                var loginResult2 = await authService.LoginAsync(new LoginRequest
                {
                    Username = username2,
                    Password = password2
                });

                if (!loginResult2.Success)
                {
                    Console.WriteLine($"Login failed: {loginResult2.ErrorMessage}");
                    return;
                }

                Console.WriteLine($"Logged in as: {loginResult2.FullName}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"\nError occurred: {ex.Message}");
            }
        }
    }
}