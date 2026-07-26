using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.ViewModels.Job
{
    public class JobDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public string? Category { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Currency { get; set; }
        public string? SkillsRequired { get; set; }
        public string? ExperienceLevel { get; set; }
        public string? Duration { get; set; }
        public int PositionsAvailable { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public JobStatus Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int ApplicantCount { get; set; }
        public string StatusDisplay => Status.ToString();
        public string EmploymentTypeDisplay => EmploymentType.ToString();

        public string BudgetDisplay
        {
            get
            {
                if (MinSalary.HasValue && MaxSalary.HasValue)
                    return $"{MinSalary:N0} — {MaxSalary:N0} {Currency}";
                if (MinSalary.HasValue)
                    return $"From {MinSalary:N0} {Currency}";
                if (MaxSalary.HasValue)
                    return $"Up to {MaxSalary:N0} {Currency}";
                return "Not disclosed";
            }
        }

        public List<string> SkillsList => (SkillsRequired ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
