using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeX.CompanyPanel.Models
{
    public enum HireStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed,
        Cancelled
    }

    public class Hire
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }

        public int CompanyId { get; set; }

        public int JobId { get; set; }

        [StringLength(1000)]
        public string? OfferLetterPath { get; set; }

        [StringLength(1000)]
        public string? ContractPath { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalaryOffered { get; set; }

        public HireStatus Status { get; set; } = HireStatus.Pending;

        public DateTime OfferedAt { get; set; } = DateTime.UtcNow;

        public DateTime? AcceptedAt { get; set; }

        [StringLength(2000)]
        public string? Remarks { get; set; }

        [ForeignKey(nameof(ApplicantId))]
        public Applicant Applicant { get; set; } = null!;

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;
    }
}
