using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;

namespace SafeX.CompanyPanel.Repositories.Implementations
{
    public class HireRepository : Repository<Hire>, IHireRepository
    {
        public HireRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Hire>> GetHiresByCompanyIdAsync(int companyId)
        {
            return await _dbSet
                .Include(h => h.Applicant)
                .Include(h => h.Job)
                .Where(h => h.CompanyId == companyId)
                .OrderByDescending(h => h.OfferedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Hire>> GetHiresByJobIdAsync(int jobId)
        {
            return await _dbSet
                .Include(h => h.Applicant)
                .Where(h => h.JobId == jobId)
                .OrderByDescending(h => h.OfferedAt)
                .ToListAsync();
        }

        public async Task<Hire?> GetHireByApplicantIdAsync(int applicantId)
        {
            return await _dbSet
                .Include(h => h.Applicant)
                .Include(h => h.Job)
                .FirstOrDefaultAsync(h => h.ApplicantId == applicantId);
        }

        public async Task<int> GetHireCountByCompanyIdAsync(int companyId)
        {
            return await _dbSet.CountAsync(h => h.CompanyId == companyId);
        }
    }
}
