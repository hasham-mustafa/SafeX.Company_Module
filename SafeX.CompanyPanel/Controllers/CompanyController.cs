using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Company;

namespace SafeX.CompanyPanel.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Verification()
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var company = await _companyService.GetCompanyByIdAsync(companyId.Value);
            var verifications = await _companyService.GetVerificationHistoryAsync(companyId.Value);

            ViewBag.CompanyLogo = company?.LogoPath;
            ViewBag.IsVerified = company?.IsVerified ?? false;
            ViewBag.Verifications = verifications;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verification(CompanyVerificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var companyId = GetCompanyId();
                if (companyId != null)
                {
                    var company = await _companyService.GetCompanyByIdAsync(companyId.Value);
                    ViewBag.CompanyLogo = company?.LogoPath;
                    ViewBag.IsVerified = company?.IsVerified ?? false;
                    ViewBag.Verifications = await _companyService.GetVerificationHistoryAsync(companyId.Value);
                }
                return View(model);
            }

            var companyIdValue = GetCompanyId();
            if (companyIdValue == null) return Unauthorized();

            var result = await _companyService.SubmitVerificationAsync(companyIdValue.Value, model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                var company = await _companyService.GetCompanyByIdAsync(companyIdValue.Value);
                ViewBag.CompanyLogo = company?.LogoPath;
                ViewBag.IsVerified = company?.IsVerified ?? false;
                ViewBag.Verifications = await _companyService.GetVerificationHistoryAsync(companyIdValue.Value);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Verification));
        }

        private int? GetCompanyId()
        {
            var claim = User.FindFirst("CompanyId")?.Value;
            return claim != null ? int.Parse(claim) : null;
        }
    }
}
