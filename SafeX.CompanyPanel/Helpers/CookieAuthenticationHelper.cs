using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SafeX.CompanyPanel.Helpers
{
    public static class CookieAuthenticationHelper
    {
        public const string AuthenticationScheme = "SafeXCompanyAuth";

        public static ClaimsPrincipal CreatePrincipal(
            int companyId, string email, string companyName, string? logoPath = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, companyId.ToString()),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, Constants.CompanyRole),
                new Claim("CompanyName", companyName),
                new Claim("CompanyId", companyId.ToString()),
                new Claim("CompanyLogo", logoPath ?? string.Empty),
                new Claim("Email", email)
            };

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public static AuthenticationTicket CreateTicket(
            int companyId, string email, string companyName, string? logoPath = null)
        {
            var principal = CreatePrincipal(companyId, email, companyName, logoPath);
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                AllowRefresh = true
            };

            return new AuthenticationTicket(principal, properties, AuthenticationScheme);
        }
    }
}
