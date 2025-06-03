using System.Security.Cryptography;
using System.Text;

namespace ExamPracticeJuneThird.Services.Helpers
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToUpper();
            }
        }
    }
}
