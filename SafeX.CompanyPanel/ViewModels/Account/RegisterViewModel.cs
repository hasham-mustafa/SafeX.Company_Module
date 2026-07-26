using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SafeX.CompanyPanel.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = null!;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(30)]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [StringLength(200)]
        [Display(Name = "Industry")]
        public string? Industry { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(500)]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(2000)]
        [Display(Name = "Company Description")]
        public string? Description { get; set; }

        [Display(Name = "Company Logo")]
        public IFormFile? Logo { get; set; }
    }
}
