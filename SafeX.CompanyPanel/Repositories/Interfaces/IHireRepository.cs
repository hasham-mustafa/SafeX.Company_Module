using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.Repositories.Interfaces
{
    public interface IHireRepository : IRepository<Hire>
    {
        Task<IEnumerable<Hire>> GetHiresByCompanyIdAsync(int companyId);
        Task<IEnumerable<Hire>> GetHiresByJobIdAsync(int jobId);
        Task<Hire?> GetHireByApplicantIdAsync(int applicantId);
        Task<int> GetHireCountByCompanyIdAsync(int companyId);
    }
}
