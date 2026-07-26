using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.ViewModels.Job;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetCompanyJobsAsync(int companyId);
        Task<Job?> GetJobByIdAsync(int jobId, int companyId);
        Task<JobResult> CreateJobAsync(int companyId, JobCreateViewModel model);
        Task<JobResult> UpdateJobAsync(int jobId, int companyId, JobEditViewModel model);
        Task<JobResult> DeleteJobAsync(int jobId, int companyId);
        Task<JobResult> PublishJobAsync(int jobId, int companyId);
        Task<JobResult> CloseJobAsync(int jobId, int companyId);
        Task<JobDetailsViewModel?> GetJobDetailsAsync(int jobId, int companyId);
        Task<IEnumerable<Job>> GetPublishedJobsAsync(int companyId);

        Task<JobListViewModel> GetFilteredJobsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            string? typeFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10);
    }

    public class JobResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? JobId { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
