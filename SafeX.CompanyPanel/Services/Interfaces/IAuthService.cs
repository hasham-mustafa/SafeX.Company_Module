using SafeX.CompanyPanel.ViewModels.Account;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterViewModel model);
        Task<AuthResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<AuthResult> ForgotPasswordAsync(ForgotPasswordViewModel model);
        Task<AuthResult> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<CompanyProfileResult> GetProfileAsync(int companyId);
        Task<AuthResult> UpdateProfileAsync(int companyId, CompanyProfileViewModel model);
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class CompanyProfileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CompanyProfileViewModel? Profile { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
