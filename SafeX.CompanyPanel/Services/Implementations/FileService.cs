using Microsoft.AspNetCore.Http;
using SafeX.CompanyPanel.Helpers;
using SafeX.CompanyPanel.Services.Interfaces;

namespace SafeX.CompanyPanel.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> SaveFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                return null;

            if (file.Length > Constants.MaxFileSizeBytes)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, subFolder);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid():N}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{subFolder}/{uniqueFileName}";
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public string GetFileUrl(string relativePath)
        {
            return relativePath ?? string.Empty;
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = Constants.AllowedImageExtensions.Split(',');
            return allowedExtensions.Contains(extension);
        }

        public bool IsValidDocumentFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = Constants.AllowedDocumentExtensions.Split(',');
            return allowedExtensions.Contains(extension);
        }
    }
}
