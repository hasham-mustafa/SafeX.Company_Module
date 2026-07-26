using System.ComponentModel.DataAnnotations;
using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.ViewModels.Job
{
    public class JobEditViewModel
    {
        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(300, MinimumLength = 5)]
        [Display(Name = "Job Title")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Job description is required.")]
        [StringLength(10000, MinimumLength = 20)]
        [Display(Name = "Description")]
        public string Description { get; set; } = null!;

        [StringLength(500)]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Employment type is required.")]
        [Display(Name = "Employment Type")]
        public EmploymentType EmploymentType { get; set; } = EmploymentType.Internship;

        [StringLength(200)]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Minimum Budget")]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Maximum Budget")]
        public decimal? MaxSalary { get; set; }

        [StringLength(10)]
        [Display(Name = "Currency")]
        public string? Currency { get; set; } = "PKR";

        [StringLength(2000)]
        [Display(Name = "Required Skills")]
        public string? SkillsRequired { get; set; }

        [StringLength(100)]
        [Display(Name = "Experience Level")]
        public string? ExperienceLevel { get; set; }

        [StringLength(100)]
        [Display(Name = "Duration")]
        public string? Duration { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Positions Available")]
        public int PositionsAvailable { get; set; } = 1;

        [Display(Name = "Application Deadline")]
        public DateTime? ApplicationDeadline { get; set; }
    }
}
