using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.ViewModels.Dashboard;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface IHireService
    {
        Task<IEnumerable<Hire>> GetCompanyHiresAsync(int companyId);
        Task<Hire?> GetHireByIdAsync(int hireId, int companyId);
        Task<HireResult> OfferJobAsync(int applicantId, int companyId, int jobId, OfferJobViewModel model);
        Task<HireResult> CancelOfferAsync(int hireId, int companyId);
        Task<bool> IsApplicantHiredAsync(int applicantId);
    }

    public class HireResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? HireId { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
