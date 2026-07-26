using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;

namespace SafeX.CompanyPanel.Repositories.Implementations
{
    public class ApplicantRepository : Repository<Applicant>, IApplicantRepository
    {
        public ApplicantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsByJobIdAsync(int jobId)
        {
            return await _dbSet
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == companyId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<int> GetApplicantCountByJobIdAsync(int jobId)
        {
            return await _dbSet.CountAsync(a => a.JobId == jobId);
        }

        public async Task<int> GetApplicantCountByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Include(a => a.Job)
                .CountAsync(a => a.Job.CompanyId == companyId);
        }

        public async Task<(IEnumerable<Applicant> Items, int TotalCount)> GetFilteredApplicantsAsync(
            int companyId,
            string? searchTerm = null,
            string? statusFilter = null,
            int? jobIdFilter = null,
            string? sortBy = "newest",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _dbSet
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(a =>
                    a.FirstName.ToLower().Contains(term) ||
                    a.LastName.ToLower().Contains(term) ||
                    a.Email.ToLower().Contains(term) ||
                    (a.Skills != null && a.Skills.ToLower().Contains(term)) ||
                    (a.University != null && a.University.ToLower().Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) &&
                Enum.TryParse<ApplicantStatus>(statusFilter, true, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            if (jobIdFilter.HasValue && jobIdFilter.Value > 0)
            {
                query = query.Where(a => a.JobId == jobIdFilter.Value);
            }

            var totalCount = await query.CountAsync();

            query = sortBy?.ToLower() switch
            {
                "oldest" => query.OrderBy(a => a.AppliedAt),
                "name" => query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName),
                "status" => query.OrderBy(a => a.Status),
                _ => query.OrderByDescending(a => a.AppliedAt)
            };

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
