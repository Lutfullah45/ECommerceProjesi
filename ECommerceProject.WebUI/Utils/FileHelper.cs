using Microsoft.AspNetCore.Http; 
using System;
using System.IO; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Hosting; 

namespace ECommerceProjesi.WebUI.Utils
{
    public class FileHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0)
            {
                return null; 
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "images", subfolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 4) + "-" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return Path.Combine("/images/", subfolder, uniqueFileName).Replace("\\", "/");
        }
        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fullPath = Path.Combine(wwwRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath); 
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Dosya silinirken hata: {ex.Message}");
                }
            }
        }
    }
}