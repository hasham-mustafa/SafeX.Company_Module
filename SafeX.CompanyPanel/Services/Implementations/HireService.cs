using Microsoft.EntityFrameworkCore;
using SafeX.CompanyPanel.Data;
using SafeX.CompanyPanel.Models;
using SafeX.CompanyPanel.Repositories.Interfaces;
using SafeX.CompanyPanel.Services.Interfaces;
using SafeX.CompanyPanel.ViewModels.Dashboard;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class HireService : IHireService
    {
        private readonly IHireRepository _hireRepository;
        private readonly IApplicantRepository _applicantRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ApplicationDbContext _context;

        public HireService(
            IHireRepository hireRepository,
            IApplicantRepository applicantRepository,
            IJobRepository jobRepository,
            ICompanyRepository companyRepository,
            ApplicationDbContext context)
        {
            _hireRepository = hireRepository;
            _applicantRepository = applicantRepository;
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _context = context;
        }

        public async Task<IEnumerable<Hire>> GetCompanyHiresAsync(int companyId)
        {
            return await _hireRepository.GetHiresByCompanyIdAsync(companyId);
        }

        public async Task<Hire?> GetHireByIdAsync(int hireId, int companyId)
        {
            return await _hireRepository
                .FirstOrDefaultAsync(h => h.Id == hireId && h.CompanyId == companyId);
        }

        public async Task<HireResult> OfferJobAsync(int applicantId, int companyId, int jobId, OfferJobViewModel model)
        {
            var result = new HireResult();

            var applicant = await _applicantRepository.GetByIdAsync(applicantId);
            if (applicant == null)
            {
                result.Errors.Add("Applicant not found.");
                return result;
            }

            var job = await _jobRepository.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);
            if (job == null)
            {
                result.Errors.Add("Job not found or does not belong to your company.");
                return result;
            }

            var existingHire = await _hireRepository.GetHireByApplicantIdAsync(applicantId);
            if (existingHire != null)
            {
                result.Errors.Add("This applicant has already been hired or has an offer.");
                return result;
            }

            var hire = new Hire
            {
                ApplicantId = applicantId,
                CompanyId = companyId,
                JobId = jobId,
                SalaryOffered = model.SalaryOffered,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Remarks = model.Remarks,
                Status = HireStatus.Pending,
                OfferedAt = DateTime.UtcNow
            };

            applicant.Status = ApplicantStatus.Hired;

            if (job.PositionsAvailable <= 1)
            {
                job.Status = JobStatus.Filled;
            }

            await _hireRepository.AddAsync(hire);
            _applicantRepository.Update(applicant);
            _jobRepository.Update(job);
            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job offer sent successfully.";
            result.HireId = hire.Id;
            return result;
        }

        public async Task<HireResult> CancelOfferAsync(int hireId, int companyId)
        {
            var result = new HireResult();
            var hire = await _hireRepository
                .FirstOrDefaultAsync(h => h.Id == hireId && h.CompanyId == companyId);

            if (hire == null)
            {
                result.Errors.Add("Hire record not found.");
                return result;
            }

            hire.Status = HireStatus.Cancelled;
            _hireRepository.Update(hire);

            var applicant = await _applicantRepository.GetByIdAsync(hire.ApplicantId);
            if (applicant != null)
            {
                applicant.Status = ApplicantStatus.Shortlisted;
                _applicantRepository.Update(applicant);
            }

            await _context.SaveChangesAsync();

            result.Success = true;
            result.Message = "Job offer cancelled.";
            return result;
        }

        public async Task<bool> IsApplicantHiredAsync(int applicantId)
        {
            var hire = await _hireRepository.GetHireByApplicantIdAsync(applicantId);
            return hire != null && hire.Status != HireStatus.Cancelled;
        }
    }
}
