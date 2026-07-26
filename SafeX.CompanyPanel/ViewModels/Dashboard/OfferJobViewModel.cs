using System.ComponentModel.DataAnnotations;

namespace SafeX.CompanyPanel.ViewModels.Dashboard
{
    public class OfferJobViewModel
    {
        [Range(0, double.MaxValue)]
        [Display(Name = "Salary Offered")]
        public decimal? SalaryOffered { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [StringLength(2000)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }
    }
}
