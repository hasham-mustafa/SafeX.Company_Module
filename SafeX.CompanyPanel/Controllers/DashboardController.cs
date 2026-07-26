using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;

namespace SafeX.CompanyPanel.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IJobService _jobService;
        private readonly IApplicantService _applicantService;
        private readonly IHireService _hireService;

        public DashboardController(
            ICompanyService companyService,
            IJobService jobService,
            IApplicantService applicantService,
            IHireService hireService)
        {
            _companyService = companyService;
            _jobService = jobService;
            _applicantService = applicantService;
            _hireService = hireService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var stats = await _companyService.GetDashboardStatsAsync(companyId.Value);
            return View(stats);
        }

        [HttpGet]
        public async Task<IActionResult> Hires()
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var hires = await _hireService.GetCompanyHiresAsync(companyId.Value);
            return View(hires);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelHire(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _hireService.CancelOfferAsync(id, companyId.Value);

            if (result.Success)
                TempData[Constants.TempDataSuccess] = result.Message;
            else
                TempData[Constants.TempDataError] = result.Message;

            return RedirectToAction(nameof(Hires));
        }

        private int? GetCompanyId()
        {
            var claim = User.FindFirst("CompanyId")?.Value;
            return claim != null ? int.Parse(claim) : null;
        }
    }
}
