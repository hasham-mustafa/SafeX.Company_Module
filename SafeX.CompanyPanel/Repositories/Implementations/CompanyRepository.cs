using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;

namespace SafeX.CompanyPanel.Repositories.Implementations
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Company?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Company?> GetCompanyWithVerificationsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.CompanyVerifications)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithJobsAsync(int companyId)
        {
            return await _dbSet
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return !await _dbSet.AnyAsync(c => c.Email == email && c.Id != excludeId.Value);
            }
            return !await _dbSet.AnyAsync(c => c.Email == email);
        }
    }
}
