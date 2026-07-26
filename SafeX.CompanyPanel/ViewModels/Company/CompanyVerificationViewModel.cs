using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SafeX.CompanyPanel.ViewModels.Company
{
    public class CompanyVerificationViewModel
    {
        [Display(Name = "Company Logo")]
        public IFormFile? Logo { get; set; }

        [Display(Name = "Business License")]
        public IFormFile? BusinessLicense { get; set; }

        [Display(Name = "Tax Registration Certificate")]
        public IFormFile? TaxCertificate { get; set; }

        [Display(Name = "Owner CNIC")]
        public IFormFile? OwnerCnic { get; set; }

        [Display(Name = "Additional Supporting Documents")]
        public IFormFile? AdditionalDocument { get; set; }
    }

    public class VerificationDocumentInfo
    {
        public int Id { get; set; }
        public string DocumentType { get; set; } = null!;
        public string DocumentPath { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? Remarks { get; set; }
    }
}
