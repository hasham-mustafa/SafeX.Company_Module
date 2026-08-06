using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Implementations;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Account;
using Xunit;

namespace SafeX.CompanyPanel.Tests.Services
{
    public class AuthServiceTests
    {
        // Each test gets its own isolated in-memory database so tests can
        // run in parallel without stepping on each other's data.
        private static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        // Builds an IHttpContextAccessor whose HttpContext resolves a mocked
        // IAuthenticationService, so SignInAsync/SignOutAsync can be verified
        // without a real ASP.NET Core pipeline running.
        private static (Mock<IHttpContextAccessor> accessorMock, Mock<IAuthenticationService> authServiceMock)
            CreateHttpContextWithAuthService()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(a => a.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<System.Security.Claims.ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object
            };

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            return (accessorMock, authServiceMock);
        }

        private static Company BuildCompany(string email, string plainPassword, bool isActive = true)
        {
            return new Company
            {
                Id = 1,
                CompanyName = "Acme Robotics",
                Email = email,
                PasswordHash = PasswordHelper.HashPassword(plainPassword),
                IsActive = isActive,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task RegisterAsync_ReturnsError_WhenEmailAlreadyExists()
        {
            // Arrange
            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock
                .Setup(r => r.IsEmailUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false); // false = NOT unique, i.e. already taken

            var (accessorMock, _) = CreateHttpContextWithAuthService();
            using var context = CreateContext();

            var sut = new AuthService(
                companyRepoMock.Object,
                accessorMock.Object,
                Mock.Of<IFileService>(),
                context);

            var model = new RegisterViewModel
            {
                CompanyName = "Acme Robotics",
                Email = "taken@acme.com",
                Password = "ValidPass123!",
                ConfirmPassword = "ValidPass123!"
            };

            // Act
            var result = await sut.RegisterAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("already registered"));
            Assert.Empty(context.Companies); // nothing should have been persisted
        }

        [Fact]
        public async Task RegisterAsync_HashesPasswordAndPersistsCompany_WhenEmailIsUnique()
        {
            // Arrange
            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock
                .Setup(r => r.IsEmailUniqueAsync(It.IsAny<string>(), null))
                .ReturnsAsync(true); // true = unique, free to register

            var (accessorMock, _) = CreateHttpContextWithAuthService();
            using var context = CreateContext();

            var sut = new AuthService(
                companyRepoMock.Object,
                accessorMock.Object,
                Mock.Of<IFileService>(),
                context);

            var model = new RegisterViewModel
            {
                CompanyName = "Acme Robotics",
                Email = "new@acme.com",
                Password = "ValidPass123!",
                ConfirmPassword = "ValidPass123!"
            };

            // Act
            var result = await sut.RegisterAsync(model);

            // Assert
            Assert.True(result.Success);
            var saved = Assert.Single(context.Companies);
            Assert.Equal("new@acme.com", saved.Email);
            // The critical security check: the plaintext password must never
            // be what's stored, and it must verify correctly through BCrypt.
            Assert.NotEqual("ValidPass123!", saved.PasswordHash);
            Assert.True(PasswordHelper.VerifyPassword("ValidPass123!", saved.PasswordHash));
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenCompanyNotFound()
        {
            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Company?)null);

            var (accessorMock, authServiceMock) = CreateHttpContextWithAuthService();
            using var context = CreateContext();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.LoginAsync(new LoginViewModel
            {
                Email = "ghost@acme.com",
                Password = "whatever"
            });

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Invalid email or password"));
            // Nobody should ever be signed in on a failed login.
            authServiceMock.Verify(a => a.SignInAsync(
                It.IsAny<HttpContext>(), It.IsAny<string>(),
                It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenAccountIsDeactivated()
        {
            var company = BuildCompany("deactivated@acme.com", "ValidPass123!", isActive: false);

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(r => r.GetByEmailAsync(company.Email)).ReturnsAsync(company);

            var (accessorMock, authServiceMock) = CreateHttpContextWithAuthService();
            using var context = CreateContext();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.LoginAsync(new LoginViewModel
            {
                Email = company.Email,
                Password = "ValidPass123!"
            });

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("deactivated"));
            authServiceMock.Verify(a => a.SignInAsync(
                It.IsAny<HttpContext>(), It.IsAny<string>(),
                It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ReturnsError_WhenPasswordIsIncorrect()
        {
            var company = BuildCompany("valid@acme.com", "ValidPass123!");

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(r => r.GetByEmailAsync(company.Email)).ReturnsAsync(company);

            var (accessorMock, authServiceMock) = CreateHttpContextWithAuthService();
            using var context = CreateContext();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.LoginAsync(new LoginViewModel
            {
                Email = company.Email,
                Password = "TotallyWrongPassword"
            });

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Invalid email or password"));
            authServiceMock.Verify(a => a.SignInAsync(
                It.IsAny<HttpContext>(), It.IsAny<string>(),
                It.IsAny<System.Security.Claims.ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_SignsInAndReturnsSuccess_WhenCredentialsAreValid()
        {
            using var context = CreateContext();
            var company = BuildCompany("valid@acme.com", "ValidPass123!");

            // Seed the company as EF would have it, so the Update()/SaveChangesAsync()
            // calls inside LoginAsync behave exactly like they would in production.
            context.Companies.Add(company);
            await context.SaveChangesAsync();

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(r => r.GetByEmailAsync(company.Email)).ReturnsAsync(company);

            var (accessorMock, authServiceMock) = CreateHttpContextWithAuthService();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.LoginAsync(new LoginViewModel
            {
                Email = company.Email,
                Password = "ValidPass123!",
                RememberMe = false
            });

            Assert.True(result.Success);
            Assert.Equal(company.Id, result.CompanyId);
            authServiceMock.Verify(a => a.SignInAsync(
                It.IsAny<HttpContext>(),
                CookieAuthenticationHelper.AuthenticationScheme,
                It.IsAny<System.Security.Claims.ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()),
                Times.Once);
        }

        [Fact]
        public async Task ResetPasswordAsync_ReturnsError_WhenTokenIsInvalidOrExpired()
        {
            using var context = CreateContext();
            var company = BuildCompany("valid@acme.com", "OldPass123!");
            company.PasswordResetToken = "expired-token";
            company.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(-1); // already expired

            context.Companies.Add(company);
            await context.SaveChangesAsync();

            var companyRepoMock = new Mock<ICompanyRepository>();
            var (accessorMock, _) = CreateHttpContextWithAuthService();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.ResetPasswordAsync(new ResetPasswordViewModel
            {
                Token = "expired-token",
                Password = "NewPass123!",
                ConfirmPassword = "NewPass123!"
            });

            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Invalid or expired"));
        }

        [Fact]
        public async Task ResetPasswordAsync_UpdatesPasswordAndClearsToken_WhenTokenIsValid()
        {
            using var context = CreateContext();
            var company = BuildCompany("valid@acme.com", "OldPass123!");
            company.PasswordResetToken = "good-token";
            company.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            context.Companies.Add(company);
            await context.SaveChangesAsync();

            var companyRepoMock = new Mock<ICompanyRepository>();
            var (accessorMock, _) = CreateHttpContextWithAuthService();

            var sut = new AuthService(companyRepoMock.Object, accessorMock.Object, Mock.Of<IFileService>(), context);

            var result = await sut.ResetPasswordAsync(new ResetPasswordViewModel
            {
                Token = "good-token",
                Password = "NewPass123!",
                ConfirmPassword = "NewPass123!"
            });

            Assert.True(result.Success);

            var updated = await context.Companies.FindAsync(company.Id);
            Assert.NotNull(updated);
            Assert.Null(updated!.PasswordResetToken);
            Assert.Null(updated.PasswordResetTokenExpiry);
            Assert.True(PasswordHelper.VerifyPassword("NewPass123!", updated.PasswordHash));
        }
    }
}
