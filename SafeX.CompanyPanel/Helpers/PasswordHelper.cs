namespace SafeX.CompanyPanel.Helpers
{
    public static class PasswordHelper
    {
        private const int WorkFactor = 12;

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static string GenerateResetToken()
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));
        }
    }
}
