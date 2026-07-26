namespace SafeX.CompanyPanel.Helpers
{
    public static class Constants
    {
        public const string CompanyRole = "Company";
        public const string AdminRole = "Admin";

        public const string SessionKeyCompanyId = "CompanyId";
        public const string SessionKeyCompanyName = "CompanyName";
        public const string SessionKeyCompanyEmail = "CompanyEmail";

        public const string TempDataSuccess = "SuccessMessage";
        public const string TempDataError = "ErrorMessage";
        public const string TempDataWarning = "WarningMessage";

        public const string UploadsLogos = "uploads/logos";
        public const string UploadsDocuments = "uploads/documents";
        public const string UploadsResumes = "uploads/resumes";

        public const long MaxFileSizeBytes = 5 * 1024 * 1024;
        public const string AllowedImageExtensions = ".jpg,.jpeg,.png,.gif,.webp";
        public const string AllowedDocumentExtensions = ".pdf,.doc,.docx,.txt";
    }
}
