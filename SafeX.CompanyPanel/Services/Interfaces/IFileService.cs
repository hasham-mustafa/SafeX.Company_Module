using Microsoft.AspNetCore.Http;

namespace SafeX.CompanyPanel.Services.Interfaces
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile file, string subFolder);
        Task<bool> DeleteFileAsync(string filePath);
        string GetFileUrl(string relativePath);
        bool IsValidImageFile(IFormFile file);
        bool IsValidDocumentFile(IFormFile file);
    }
}
