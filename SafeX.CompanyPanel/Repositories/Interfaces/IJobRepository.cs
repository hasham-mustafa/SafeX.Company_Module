using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Repositories.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(int companyId);
        Task<IEnumerable<Job>> GetPublishedJobsByCompanyIdAsync(int companyId);
        Task<Job?> GetJobWithApplicantsAsync(int jobId);
        Task<int> GetJobCountByCompanyIdAsync(int companyId);
        Task<int> GetActiveJobCountByCompanyIdAsync(int companyId);

        Task<(IEnumerable<Job> Items, int TotalCount)> GetFilteredJobsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            string? typeFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10);
    }
}
