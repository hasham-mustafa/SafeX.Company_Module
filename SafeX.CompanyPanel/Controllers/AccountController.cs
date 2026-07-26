using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Account;

namespace SafeX.CompanyPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ForgotPasswordAsync(model);
            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return NotFound();

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ResetPasswordAsync(model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _authService.GetProfileAsync(companyId.Value);
            if (!result.Success || result.Profile == null)
                return NotFound();

            return View(result.Profile);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(CompanyProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _authService.UpdateProfileAsync(companyId.Value, model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private int? GetCompanyId()
        {
            var claim = User.FindFirst("CompanyId")?.Value;
            return claim != null ? int.Parse(claim) : null;
        }
    }
}
