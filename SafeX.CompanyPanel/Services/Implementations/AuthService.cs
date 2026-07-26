using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Account;
using Microsoft.EntityFrameworkCore;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;

        public AuthService(
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor,
            IFileService fileService,
            ApplicationDbContext context)
        {
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
            _context = context;
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
        {
            var result = new AuthResult();

            var emailExists = await _companyRepository.IsEmailUniqueAsync(model.Email);
            if (!emailExists)
            {
                result.Errors.Add("A company with this email is already registered.");
                return result;
            }

            string? logoPath = null;
            if (model.Logo != null && model.Logo.Length > 0)
            {
                if (!_fileService.IsValidImageFile(model.Logo))
                {
                    result.Errors.Add("Logo must be a valid image file (JPG, PNG, GIF, WebP) under 5MB.");
                    return result;
                }
                logoPath = await _fileService.SaveFileAsync(model.Logo, Constants.UploadsLogos);
            }

            var company = new Company
            {
                CompanyName = model.CompanyName,
                Email = model.Email,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                Phone = model.Phone,
                Industry = model.Industry,
                Website = model.Website,
                Address = model.Address,
                Description = model.Description,
                LogoPath = logoPath,
                IsActive = true,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Registration successful! You can now log in.";
            result.CompanyId = company.Id;
            return result;
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel model)
        {
            var result = new AuthResult();

            var company = await _companyRepository.GetByEmailAsync(model.Email);
            if (company == null)
            {
                result.Errors.Add("Invalid email or password.");
                return result;
            }

            if (!company.IsActive)
            {
                result.Errors.Add("Your account has been deactivated. Please contact support.");
                return result;
            }

            if (!PasswordHelper.VerifyPassword(model.Password, company.PasswordHash))
            {
                result.Errors.Add("Invalid email or password.");
                return result;
            }

            company.LastLoginAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            var principal = CookieAuthenticationHelper.CreatePrincipal(
                company.Id, company.Email, company.CompanyName, company.LogoPath);

            var properties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(model.RememberMe ? 30 : 7),
                AllowRefresh = true
            };

            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationHelper.AuthenticationScheme,
                    principal,
                    properties);
            }

            result.Success = true;
            result.Message = "Login successful. Welcome back!";
            result.CompanyId = company.Id;
            result.CompanyName = company.CompanyName;
            result.CompanyEmail = company.Email;
            return result;
        }

        public async Task LogoutAsync()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(
                    CookieAuthenticationHelper.AuthenticationScheme);
            }
        }

        public async Task<AuthResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            var result = new AuthResult();
            var company = await _companyRepository.GetByEmailAsync(model.Email);

            if (company == null)
            {
                result.Success = true;
                result.Message = "If this email is registered, a reset link has been sent.";
                return result;
            }

            company.PasswordResetToken = PasswordHelper.GenerateResetToken();
            company.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "If this email is registered, a reset link has been sent.";
            return result;
        }

        public async Task<AuthResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var result = new AuthResult();
            var company = await _context.Companies
                .FirstOrDefaultAsync(c =>
                    c.PasswordResetToken == model.Token &&
                    c.PasswordResetTokenExpiry > DateTime.UtcNow);

            if (company == null)
            {
                result.Errors.Add("Invalid or expired reset token. Please request a new one.");
                return result;
            }

            company.PasswordHash = PasswordHelper.HashPassword(model.Password);
            company.PasswordResetToken = null;
            company.PasswordResetTokenExpiry = null;
            company.UpdatedAt = DateTime.UtcNow;
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Password has been reset successfully. Please log in.";
            return result;
        }

        public async Task<CompanyProfileResult> GetProfileAsync(int companyId)
        {
            var result = new CompanyProfileResult();
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company == null)
            {
                result.Errors.Add("Company not found.");
                return result;
            }

            result.Success = true;
            result.Profile = new CompanyProfileViewModel
            {
                CompanyName = company.CompanyName,
                Email = company.Email,
                Phone = company.Phone,
                Industry = company.Industry,
                Website = company.Website,
                Address = company.Address,
                Description = company.Description,
                LogoPath = company.LogoPath,
                IsVerified = company.IsVerified
            };

            return result;
        }

        public async Task<AuthResult> UpdateProfileAsync(int companyId, CompanyProfileViewModel model)
        {
            var result = new AuthResult();
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company == null)
            {
                result.Errors.Add("Company not found.");
                return result;
            }

            var emailUnique = await _companyRepository.IsEmailUniqueAsync(model.Email, companyId);
            if (!emailUnique)
            {
                result.Errors.Add("This email is already in use by another company.");
                return result;
            }

            string? logoPath = company.LogoPath;
            if (model.Logo != null && model.Logo.Length > 0)
            {
                if (!_fileService.IsValidImageFile(model.Logo))
                {
                    result.Errors.Add("Logo must be a valid image file (JPG, PNG, GIF, WebP) under 5MB.");
                    return result;
                }

                if (!string.IsNullOrEmpty(company.LogoPath))
                {
                    await _fileService.DeleteFileAsync(company.LogoPath);
                }

                logoPath = await _fileService.SaveFileAsync(model.Logo, Constants.UploadsLogos);
            }

            company.CompanyName = model.CompanyName;
            company.Email = model.Email;
            company.Phone = model.Phone;
            company.Industry = model.Industry;
            company.Website = model.Website;
            company.Address = model.Address;
            company.Description = model.Description;
            company.LogoPath = logoPath;
            company.UpdatedAt = DateTime.UtcNow;

            _context.Companies.Update(company);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Profile updated successfully.";
            return result;
        }
    }
}
