namespace SafeX.CompanyPanel.ViewModels.Applicant
{
    public class ApplicantSearchViewModel
    {
        public IEnumerable<Models.Applicant> Applicants { get; set; } = Enumerable.Empty<Models.Applicant>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public string? SearchTerm { get; set; }
        public string? StatusFilter { get; set; }
        public int? JobIdFilter { get; set; }
        public string? SortBy { get; set; } = "newest";
    }
}
