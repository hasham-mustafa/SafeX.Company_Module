using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Repositories.Interfaces
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        Task<IEnumerable<Applicant>> GetApplicantsByJobIdAsync(int jobId);
        Task<IEnumerable<Applicant>> GetApplicantsByCompanyIdAsync(int companyId);
        Task<int> GetApplicantCountByJobIdAsync(int jobId);
        Task<int> GetApplicantCountByCompanyIdAsync(int companyId);

        Task<(IEnumerable<Applicant> Items, int TotalCount)> GetFilteredApplicantsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            int? jobIdFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10);
    }
}
