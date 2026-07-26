using System.ComponentModel.DataAnnotations;

namespace SafeX.CompanyPanel.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;
    }
}
