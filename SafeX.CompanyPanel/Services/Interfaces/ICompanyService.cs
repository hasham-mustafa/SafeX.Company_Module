using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.ViewModels.Company;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company?> GetCompanyByIdAsync(int companyId);
        Task<CompanyVerificationResult> SubmitVerificationAsync(int companyId, CompanyVerificationViewModel model);
        Task<IEnumerable<VerificationDocumentInfo>> GetVerificationHistoryAsync(int companyId);
        Task<CompanyDashboardStats> GetDashboardStatsAsync(int companyId);
    }

    public class CompanyVerificationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }

    public class CompanyDashboardStats
    {
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ClosedJobs { get; set; }
        public int TotalApplicants { get; set; }
        public int TotalHires { get; set; }
        public int PendingVerifications { get; set; }
        public string? CompanyName { get; set; }
        public string? LogoPath { get; set; }
        public bool IsVerified { get; set; }
        public List<Job> RecentJobs { get; set; } = new();
        public List<Applicant> RecentApplicants { get; set; } = new();
    }
}
