using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.ViewModels.Applicant;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface IApplicantService
    {
        Task<IEnumerable<Applicant>> GetJobApplicantsAsync(int jobId, int companyId);
        Task<IEnumerable<Applicant>> GetCompanyApplicantsAsync(int companyId);
        Task<Applicant?> GetApplicantByIdAsync(int applicantId, int companyId);
        Task<ApplicantResult> UpdateApplicantStatusAsync(int applicantId, int companyId, string status);
        Task<ApplicantResult> ShortlistApplicantAsync(int applicantId, int companyId);
        Task<ApplicantResult> RejectApplicantAsync(int applicantId, int companyId);

        Task<ApplicantSearchViewModel> GetFilteredApplicantsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            int? jobIdFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApplicantPortfolioViewModel?> GetApplicantPortfolioAsync(int applicantId, int companyId);
    }

    public class ApplicantResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
