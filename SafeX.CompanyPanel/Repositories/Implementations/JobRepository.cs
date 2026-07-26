using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;

namespace SafeX.CompanyPanel.Repositories.Implementations
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Job>> GetJobsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(j => j.CompanyId == companyId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetPublishedJobsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Where(j => j.CompanyId == companyId && j.Status == JobStatus.Published && j.IsActive)
                .OrderByDescending(j => j.PublishedAt)
                .ToListAsync();
        }

        public async Task<Job?> GetJobWithApplicantsAsync(int jobId)
        {
            return await _dbSet
                .Include(j => j.Applicants)
                .FirstOrDefaultAsync(j => j.Id == jobId);
        }

        public async Task<int> GetJobCountByCompanyIdAsync(int companyId)
        {
            return await _dbSet.CountAsync(j => j.CompanyId == companyId);
        }

        public async Task<int> GetActiveJobCountByCompanyIdAsync(int companyId)
        {
            return await _dbSet.CountAsync(j => j.CompanyId == companyId && j.IsActive);
        }

        public async Task<(IEnumerable<Job> Items, int TotalCount)> GetFilteredJobsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            string? typeFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _dbSet.Where(j => j.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(j =>
                    j.Title.ToLower().Contains(term) ||
                    j.Description.ToLower().Contains(term) ||
                    (j.SkillsRequired != null && j.SkillsRequired.ToLower().Contains(term)) ||
                    (j.Category != null && j.Category.ToLower().Contains(term)) ||
                    (j.Location != null && j.Location.ToLower().Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<JobStatus>(statusFilter, true, out var parsedStatus))
            {
                query = query.Where(j => j.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(typeFilter) &&
                Enum.TryParse<EmploymentType>(typeFilter, true, out var parsedType))
            {
                query = query.Where(j => j.EmploymentType == parsedType);
            }

            var totalCount = await query.CountAsync();

            query = sortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(j => j.CreatedAt),
                "title" => query.OrderBy(j => j.Title),
                "status" => query.OrderBy(j => j.Status),
                "deadline" => query.OrderBy(j => j.ApplicationDeadline),
                _ => query.OrderByDescending(j => j.CreatedAt)
            };

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(j => j.Applicants)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
