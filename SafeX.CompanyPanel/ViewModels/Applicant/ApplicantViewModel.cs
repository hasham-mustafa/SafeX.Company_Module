using SafeX.CompanyPanel.Models;

namespace SafeX.CompanyPanel.ViewModels.Applicant
{
    public class ApplicantViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string? University { get; set; }
        public string? Skills { get; set; }
        public string? ResumePath { get; set; }
        public string? CoverLetter { get; set; }
        public string? Proposal { get; set; }
        public decimal? BidAmount { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public ApplicantStatus Status { get; set; }
        public string StatusDisplay => Status.ToString();
        public DateTime AppliedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? JobTitle { get; set; }

        public List<string> SkillsList => (Skills ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }
}
