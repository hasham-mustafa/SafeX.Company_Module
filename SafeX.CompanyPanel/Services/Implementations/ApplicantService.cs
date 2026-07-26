using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Applicant;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ApplicationDbContext _context;

        public ApplicantService(
            IApplicantRepository applicantRepository,
            IJobRepository jobRepository,
            ICompanyRepository companyRepository,
            ApplicationDbContext context)
        {
            _applicantRepository = applicantRepository;
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _context = context;
        }

        public async Task<IEnumerable<Applicant>> GetJobApplicantsAsync(int jobId, int companyId)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);
            if (job == null) return Enumerable.Empty<Applicant>();

            return await _applicantRepository.GetApplicantsByJobIdAsync(jobId);
        }

        public async Task<IEnumerable<Applicant>> GetCompanyApplicantsAsync(int companyId)
        {
            return await _applicantRepository.GetApplicantsByCompanyIdAsync(companyId);
        }

        public async Task<Applicant?> GetApplicantByIdAsync(int applicantId, int companyId)
        {
            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null) return null;

            var job = await _jobRepository.GetByIdAsync(applicant.JobId);
            if (job == null || job.CompanyId != companyId) return null;

            return applicant;
        }

        public async Task<ApplicantResult> UpdateApplicantStatusAsync(int applicantId, int companyId, string status)
        {
            var result = new ApplicantResult();
            var applicant = await GetApplicantByIdAsync(applicantId, companyId);

            if (applicant == null)
            {
                result.Errors.Add("Applicant not found.");
                return result;
            }

            if (!Enum.TryParse<ApplicantStatus>(status, true, out var parsedStatus))
            {
                result.Errors.Add("Invalid status value.");
                return result;
            }

            applicant.Status = parsedStatus;
            applicant.ReviewedAt = DateTime.UtcNow;
            _applicantRepository.Update(applicant);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = $"Applicant status updated to {parsedStatus}.";
            return result;
        }

        public async Task<ApplicantResult> ShortlistApplicantAsync(int applicantId, int companyId)
        {
            return await UpdateApplicantStatusAsync(applicantId, companyId, "Shortlisted");
        }

        public async Task<ApplicantResult> RejectApplicantAsync(int applicantId, int companyId)
        {
            return await UpdateApplicantStatusAsync(applicantId, companyId, "Rejected");
        }

        public async Task<ApplicantSearchViewModel> GetFilteredApplicantsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            int? jobIdFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (items, totalCount) = await _applicantRepository.GetFilteredApplicantsAsync(
                companyId, searchTerm, statusFilter, jobIdFilter, sortBy, pageNumber, pageSize);

            return new ApplicantSearchViewModel
            {
                Applicants = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                StatusFilter = statusFilter,
                JobIdFilter = jobIdFilter,
                SortBy = sortBy
            };
        }

        public async Task<ApplicantPortfolioViewModel?> GetApplicantPortfolioAsync(int applicantId, int companyId)
        {
            var applicant = await _context.Applicants
                .Include(a => a.Job)
                .ThenInclude(j => j.Company)
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (applicant == null) return null;

            if (applicant.Job.CompanyId != companyId) return null;

            return new ApplicantPortfolioViewModel
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                Phone = applicant.Phone,
                ProfilePicture = applicant.ProfilePicture,
                University = applicant.University,
                Skills = applicant.Skills,
                ResumePath = applicant.ResumePath,
                CoverLetter = applicant.CoverLetter,
                Proposal = applicant.Proposal,
                BidAmount = applicant.BidAmount,
                PortfolioUrl = applicant.PortfolioUrl,
                LinkedInUrl = applicant.LinkedInUrl,
                JobTitle = applicant.Job?.Title,
                CompanyName = applicant.Job?.Company?.CompanyName,
                AppliedAt = applicant.AppliedAt,
                StatusDisplay = applicant.Status.ToString()
            };
        }
    }
}
