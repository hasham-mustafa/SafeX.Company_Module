using System.ComponentModel.DataAnnotations;

namespace SafeX.CompanyPanel.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(200, MinimumLength = 2)]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Industry { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(500)]
        public string? Website { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? LogoPath { get; set; }

        public bool IsVerified { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        [StringLength(500)]
        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiry { get; set; }

        public ICollection<CompanyVerification> CompanyVerifications { get; set; } = new List<CompanyVerification>();

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public ICollection<Hire> Hires { get; set; } = new List<Hire>();
    }
}
