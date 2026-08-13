using System.ComponentModel.DataAnnotations;

namespace SafeX.CompanyPanel.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password hash is required.")]
        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
    }
}