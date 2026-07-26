using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeX.CompanyPanel.Models
{
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        Contract,
        Internship,
        Freelance
    }

    public enum JobStatus
    {
        Draft,
        Published,
        Closed,
        Filled
    }

    public class Job
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(300, MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Job description is required.")]
        [StringLength(10000, MinimumLength = 20)]
        public string Description { get; set; } = null!;

        [StringLength(500)]
        public string? Location { get; set; }

        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        [StringLength(200)]
        public string? Category { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxSalary { get; set; }

        [StringLength(10)]
        public string? Currency { get; set; } = "PKR";

        [StringLength(2000)]
        public string? SkillsRequired { get; set; }

        [StringLength(100)]
        public string? ExperienceLevel { get; set; }

        [StringLength(100)]
        public string? Duration { get; set; }

        [Range(1, int.MaxValue)]
        public int PositionsAvailable { get; set; } = 1;

        public DateTime? ApplicationDeadline { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Draft;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PublishedAt { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();

        public ICollection<Hire> Hires { get; set; } = new List<Hire>();
    }
}
