using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByEmailAsync(string email);
        Task<Company?> GetCompanyWithVerificationsAsync(int companyId);
        Task<Company?> GetCompanyWithJobsAsync(int companyId);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
    }
}
