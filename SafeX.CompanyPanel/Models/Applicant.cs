using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeX.CompanyPanel.Models
{
    public enum ApplicantStatus
    {
        Pending,
        Reviewed,
        Shortlisted,
        Rejected,
        Hired
    }

    public class Applicant
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(500)]
        public string? ProfilePicture { get; set; }

        [StringLength(200)]
        public string? University { get; set; }

        [StringLength(2000)]
        public string? Skills { get; set; }

        [StringLength(1000)]
        public string? ResumePath { get; set; }

        [StringLength(5000)]
        public string? CoverLetter { get; set; }

        [StringLength(5000)]
        public string? Proposal { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? BidAmount { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(500)]
        public string? PortfolioUrl { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(500)]
        public string? LinkedInUrl { get; set; }

        public ApplicantStatus Status { get; set; } = ApplicantStatus.Pending;

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;

        public Hire? Hire { get; set; }
    }
}
