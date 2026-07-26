using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Job;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly ApplicationDbContext _context;

        public JobService(
            IJobRepository jobRepository,
            IApplicantRepository applicantRepository,
            ApplicationDbContext context)
        {
            _jobRepository = jobRepository;
            _applicantRepository = applicantRepository;
            _context = context;
        }

        public async Task<IEnumerable<Job>> GetCompanyJobsAsync(int companyId)
        {
            return await _jobRepository.GetJobsByCompanyIdAsync(companyId);
        }

        public async Task<Job?> GetJobByIdAsync(int jobId, int companyId)
        {
            return await _jobRepository
                .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);
        }

        public async Task<JobResult> CreateJobAsync(int companyId, JobCreateViewModel model)
        {
            var result = new JobResult();

            var job = new Job
            {
                CompanyId = companyId,
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                EmploymentType = model.EmploymentType,
                Category = model.Category,
                MinSalary = model.MinSalary,
                MaxSalary = model.MaxSalary,
                Currency = model.Currency,
                SkillsRequired = model.SkillsRequired,
                ExperienceLevel = model.ExperienceLevel,
                Duration = model.Duration,
                PositionsAvailable = model.PositionsAvailable,
                ApplicationDeadline = model.ApplicationDeadline,
                Status = JobStatus.Draft,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _jobRepository.AddAsync(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job created successfully.";
            result.JobId = job.Id;
            return result;
        }

        public async Task<JobResult> UpdateJobAsync(int jobId, int companyId, JobEditViewModel model)
        {
            var result = new JobResult();
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null)
            {
                result.Errors.Add("Job not found.");
                return result;
            }

            job.Title = model.Title;
            job.Description = model.Description;
            job.Location = model.Location;
            job.EmploymentType = model.EmploymentType;
            job.Category = model.Category;
            job.MinSalary = model.MinSalary;
            job.MaxSalary = model.MaxSalary;
            job.Currency = model.Currency;
            job.SkillsRequired = model.SkillsRequired;
            job.ExperienceLevel = model.ExperienceLevel;
            job.Duration = model.Duration;
            job.PositionsAvailable = model.PositionsAvailable;
            job.ApplicationDeadline = model.ApplicationDeadline;
            job.UpdatedAt = DateTime.UtcNow;

            _jobRepository.Update(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job updated successfully.";
            result.JobId = job.Id;
            return result;
        }

        public async Task<JobResult> DeleteJobAsync(int jobId, int companyId)
        {
            var result = new JobResult();
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null)
            {
                result.Errors.Add("Job not found.");
                return result;
            }

            job.IsActive = false;
            job.Status = JobStatus.Closed;
            _jobRepository.Update(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job deleted successfully.";
            return result;
        }

        public async Task<JobResult> PublishJobAsync(int jobId, int companyId)
        {
            var result = new JobResult();
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null)
            {
                result.Errors.Add("Job not found.");
                return result;
            }

            job.Status = JobStatus.Published;
            job.PublishedAt = DateTime.UtcNow;
            job.UpdatedAt = DateTime.UtcNow;
            _jobRepository.Update(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job published successfully.";
            return result;
        }

        public async Task<JobResult> CloseJobAsync(int jobId, int companyId)
        {
            var result = new JobResult();
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null)
            {
                result.Errors.Add("Job not found.");
                return result;
            }

            job.Status = JobStatus.Closed;
            job.UpdatedAt = DateTime.UtcNow;
            _jobRepository.Update(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job closed successfully.";
            return result;
        }

        public async Task<JobDetailsViewModel?> GetJobDetailsAsync(int jobId, int companyId)
        {
            var job = await _jobRepository
                .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null) return null;

            var applicantCount = await _applicantRepository.GetApplicantCountByJobIdAsync(jobId);

            return new JobDetailsViewModel
            {
                Id = job.Id,
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
                ApplicationDeadline = job.ApplicationDeadline,
                Status = job.Status,
                IsActive = job.IsActive,
                CreatedAt = job.CreatedAt,
                PublishedAt = job.PublishedAt,
                ApplicantCount = applicantCount
            };
        }

        public async Task<IEnumerable<Job>> GetPublishedJobsAsync(int companyId)
        {
            return await _jobRepository.GetPublishedJobsByCompanyIdAsync(companyId);
        }

        public async Task<JobListViewModel> GetFilteredJobsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            string? typeFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (items, totalCount) = await _jobRepository.GetFilteredJobsAsync(
                companyId, searchTerm, statusFilter, typeFilter, sortBy, pageNumber, pageSize);

            return new JobListViewModel
            {
                Jobs = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                StatusFilter = statusFilter,
                TypeFilter = typeFilter,
                SortBy = sortBy
            };
        }
    }
}
