using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Job;

namespace SafeX.CompanyPanel.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm = null,
            string? statusFilter = null,
            string? typeFilter = null,
            string? sortBy = "newest",
            int page = 1)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var model = await _jobService.GetFilteredJobsAsync(
                companyId.Value, searchTerm, statusFilter, typeFilter, sortBy, page, 10);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _jobService.CreateJobAsync(companyId.Value, model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var job = await _jobService.GetJobByIdAsync(id, companyId.Value);
            if (job == null) return NotFound();

            var model = new JobEditViewModel
            {
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                EmploymentType = job.EmploymentType,
                Category = job.Category,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                Currency = job.Currency,
                SkillsRequired = job.SkillsRequired,
                ExperienceLevel = job.ExperienceLevel,
                Duration = job.Duration,
                PositionsAvailable = job.PositionsAvailable,
                ApplicationDeadline = job.ApplicationDeadline
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _jobService.UpdateJobAsync(id, companyId.Value, model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(model);
            }

            TempData[Constants.TempDataSuccess] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var job = await _jobService.GetJobDetailsAsync(id, companyId.Value);
            if (job == null) return NotFound();

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _jobService.PublishJobAsync(id, companyId.Value);
            if (!result.Success)
                TempData[Constants.TempDataError] = result.Message;
            else
                TempData[Constants.TempDataSuccess] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _jobService.CloseJobAsync(id, companyId.Value);
            if (!result.Success)
                TempData[Constants.TempDataError] = result.Message;
            else
                TempData[Constants.TempDataSuccess] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var companyId = GetCompanyId();
            if (companyId == null) return Unauthorized();

            var result = await _jobService.DeleteJobAsync(id, companyId.Value);
            if (!result.Success)
                TempData[Constants.TempDataError] = result.Message;
            else
                TempData[Constants.TempDataSuccess] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        private int? GetCompanyId()
        {
            var claim = User.FindFirst("CompanyId")?.Value;
            return claim != null ? int.Parse(claim) : null;
        }
    }
}
