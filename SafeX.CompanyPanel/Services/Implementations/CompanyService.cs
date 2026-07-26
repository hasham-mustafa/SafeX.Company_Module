using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Company;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IHireRepository _hireRepository;
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;

        public CompanyService(
            ICompanyRepository companyRepository,
            IJobRepository jobRepository,
            IApplicantRepository applicantRepository,
            IHireRepository hireRepository,
            IFileService fileService,
            ApplicationDbContext context)
        {
            _companyRepository = companyRepository;
            _jobRepository = jobRepository;
            _applicantRepository = applicantRepository;
            _hireRepository = hireRepository;
            _fileService = fileService;
            _context = context;
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _companyRepository.GetByIdAsync(companyId);
        }

        public async Task<CompanyVerificationResult> SubmitVerificationAsync(
            int companyId, CompanyVerificationViewModel model)
        {
            var result = new CompanyVerificationResult();
            var company = await _companyRepository.GetCompanyWithVerificationsAsync(companyId);

            if (company == null)
            {
                result.Errors.Add("Company not found.");
                return result;
            }

            var documentUploads = new Dictionary<string, IFormFile?>
            {
                { "Company Logo", model.Logo },
                { "Business License", model.BusinessLicense },
                { "Tax Registration Certificate", model.TaxCertificate },
                { "Owner CNIC", model.OwnerCnic },
                { "Additional Supporting Document", model.AdditionalDocument }
            };

            var hasAnyDocument = false;

            foreach (var (docType, file) in documentUploads)
            {
                if (file == null || file.Length == 0) continue;

                if (!_fileService.IsValidDocumentFile(file) && !_fileService.IsValidImageFile(file))
                {
                    result.Errors.Add($"{docType}: Invalid file type. Allowed: PDF, DOC, DOCX, JPG, PNG.");
                    continue;
                }

                var filePath = await _fileService.SaveFileAsync(file, Constants.UploadsDocuments);
                if (filePath == null)
                {
                    result.Errors.Add($"{docType}: Failed to upload file.");
                    continue;
                }

                var verification = new CompanyVerification
                {
                    CompanyId = companyId,
                    DocumentType = docType,
                    DocumentPath = filePath,
                    Status = VerificationStatus.Pending,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.CompanyVerifications.Add(verification);
                hasAnyDocument = true;
            }

            if (!hasAnyDocument && result.Errors.Count == 0)
            {
                result.Errors.Add("Please upload at least one document for verification.");
                return result;
            }

            if (result.Errors.Count > 0 && !hasAnyDocument)
            {
                return result;
            }

            await _context.SaveChangesAsync();

            if (model.Logo != null && model.Logo.Length > 0)
            {
                if (!string.IsNullOrEmpty(company.LogoPath))
                {
                    await _fileService.DeleteFileAsync(company.LogoPath);
                }
                var logoPath = await _fileService.SaveFileAsync(model.Logo, Constants.UploadsLogos);
                if (logoPath != null)
                {
                    company.LogoPath = logoPath;
                    company.UpdatedAt = DateTime.UtcNow;
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                }
            }

            result.Success = true;
            result.Message = "Verification documents submitted successfully. They will be reviewed shortly.";
            return result;
        }

        public async Task<IEnumerable<VerificationDocumentInfo>> GetVerificationHistoryAsync(int companyId)
        {
            return await _context.CompanyVerifications
                .Where(v => v.CompanyId == companyId)
                .OrderByDescending(v => v.SubmittedAt)
                .Select(v => new VerificationDocumentInfo
                {
                    Id = v.Id,
                    DocumentType = v.DocumentType,
                    DocumentPath = v.DocumentPath,
                    Status = v.Status.ToString(),
                    SubmittedAt = v.SubmittedAt,
                    ReviewedAt = v.ReviewedAt,
                    Remarks = v.Remarks
                })
                .ToListAsync();
        }

        public async Task<CompanyDashboardStats> GetDashboardStatsAsync(int companyId)
        {
            var company = await _companyRepository.GetCompanyWithVerificationsAsync(companyId);

            if (company == null)
                return new CompanyDashboardStats();

            var totalJobs = await _jobRepository.GetJobCountByCompanyIdAsync(companyId);
            var activeJobs = await _context.Jobs
                .CountAsync(j => j.CompanyId == companyId && j.IsActive && j.Status == JobStatus.Published);
            var closedJobs = await _context.Jobs
                .CountAsync(j => j.CompanyId == companyId && (j.Status == JobStatus.Closed || j.Status == JobStatus.Filled));
            var totalApplicants = await _applicantRepository.GetApplicantCountByCompanyIdAsync(companyId);
            var totalHires = await _hireRepository.GetHireCountByCompanyIdAsync(companyId);
            var pendingVerifications = await _context.CompanyVerifications
                .CountAsync(v => v.CompanyId == companyId && v.Status == VerificationStatus.Pending);
            var recentJobs = await _context.Jobs
                .Where(j => j.CompanyId == companyId)
                .OrderByDescending(j => j.CreatedAt)
                .Take(5)
                .ToListAsync();
            var recentApplicants = await _context.Applicants
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == companyId)
                .OrderByDescending(a => a.AppliedAt)
                .Take(5)
                .ToListAsync();

            return new CompanyDashboardStats
            {
                TotalJobs = totalJobs,
                ActiveJobs = activeJobs,
                ClosedJobs = closedJobs,
                TotalApplicants = totalApplicants,
                TotalHires = totalHires,
                PendingVerifications = pendingVerifications,
                CompanyName = company.CompanyName,
                IsVerified = company.IsVerified,
                LogoPath = company.LogoPath,
                RecentJobs = recentJobs,
                RecentApplicants = recentApplicants
            };
        }
    }
}
