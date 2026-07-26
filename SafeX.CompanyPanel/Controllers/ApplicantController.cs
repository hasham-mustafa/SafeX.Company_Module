using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Dashboard;

namespace SafeX.CompanyPanel.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        private readonly IApplicantService _applicantService;
        private readonly IHireService _hireService;
        private readonly IJobService _jobService;

        public ApplicantController(
            IApplicantService applicantService,
            IHireService hireService,
            IJobService jobService)
        {
            _applicantService = applicantService;
            _hireService = hireService;
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            string? statusFilter = null,
            int? jobIdFilter = null,
            string? sortBy = "newest",
            int page = 1)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            ViewBag.Jobs = await _jobService.GetCompanyJobsAsync(companyId.Value);

            var model = await _applicantService.GetFilteredApplicantsAsync(
                companyId.Value, searchTerm, statusFilter, jobIdFilter, sortBy, page, 10);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> JobApplicants(int jobId)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var job = await _jobService.GetJobByIdAsync(jobId, companyId.Value);
            if (job == null) return NotFound();

            ViewBag.JobTitle = job.Title;
            ViewBag.JobId = jobId;

            var applicants = await _applicantService.GetJobApplicantsAsync(jobId, companyId.Value);
            return View(applicants);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var applicant = await _applicantService.GetApplicantByIdAsync(id, companyId.Value);
            if (applicant == null) return NotFound();

            ViewBag.JobTitle = (await _jobService.GetJobByIdAsync(applicant.JobId, companyId.Value))?.Title;
            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Shortlist(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _applicantService.ShortlistApplicantAsync(id, companyId.Value);
            if (!result.Success)
                TempData[Constants.TempDataError] = result.Message;
            else
                TempData[Constants.TempDataSuccess] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _applicantService.RejectApplicantAsync(id, companyId.Value);
            if (!result.Success)
                TempData[Constants.TempDataError] = result.Message;
            else
                TempData[Constants.TempDataSuccess] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> OfferJob(int applicantId, int jobId)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var applicant = await _applicantService.GetApplicantByIdAsync(applicantId, companyId.Value);
            if (applicant == null) return NotFound();

            var isHired = await _hireService.IsApplicantHiredAsync(applicantId);
            if (isHired)
            {
                TempData[Constants.TempDataError] = "This applicant already has an offer.";
                return RedirectToAction("JobApplicants", new { jobId });
            }

            ViewBag.ApplicantId = applicantId;
            ViewBag.JobId = jobId;
            ViewBag.ApplicantName = $"{applicant.FirstName} {applicant.LastName}";

            return View(new OfferJobViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OfferJob(int applicantId, int jobId, OfferJobViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ApplicantId = applicantId;
                ViewBag.JobId = jobId;
                return View(model);
            }

            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _hireService.OfferJobAsync(applicantId, companyId.Value, jobId, model);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                ViewBag.ApplicantId = applicantId;
                ViewBag.JobId = jobId;
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction("JobApplicants", new { jobId });
        }

        [HttpGet]
        public async Task<IActionResult> Portfolio(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var portfolio = await _applicantService.GetApplicantPortfolioAsync(id, companyId.Value);
            if (portfolio == null) return NotFound();

            return View(portfolio);
        }

        private int? GetCompanyId()
        {
            var claim = User.FindFirst("CompanyId")?.Value;
            return claim != null ? int.Parse(claim) : null;
        }
    }
}
