using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeX.CompanyPanel.Models
{
    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class CompanyVerification
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Document type is required.")]
        [StringLength(200)]
        public string DocumentType { get; set; } = null!;

        [Required(ErrorMessage = "Document path is required.")]
        [StringLength(1000)]
        public string DocumentPath { get; set; } = null!;

        public VerificationStatus Status { get; set; } = VerificationStatus.Pending;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        [StringLength(200)]
        public string? ReviewedBy { get; set; }

        [StringLength(1000)]
        public string? Remarks { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
    }
}
