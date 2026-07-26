using System.ComponentModel.DataAnnotations;

namespace SafeX.CompanyPanel.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
