using E_commerce.Abstraction.IService.Attachment;
using Microsoft.AspNetCore.Http;
using E_commerce.Domain.Exceptions; // 👈 تأكد من مسار الـ Custom Exceptions بتاعك
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace E_commerce.Services.Services
{
    public class AttachementServices : IAttachmentService
    {
        private readonly List<string> AllowedExtentions = new List<string>() { ".jpg", ".png", ".jpeg" };
        private const long _fileSizeLimit = 5 * 1024 * 1024; // 5 MB

        public async Task<string> UploadImageAsync(IFormFile? formFile, string subFolder)
        {
            // 1. فحص وجود الملف
            if (formFile == null || formFile.Length == 0)
            {
                // 👇 استخدمنا BadRequest مخصص عشان يرجع 400
                throw new BadRequestExceptionCustome("لم يتم إرسال أي ملف، أو الملف فارغ تماماً.");
            }

            // 2. فحص الامتداد (Allowed Extensions)
            var fileExtention = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtention) || !AllowedExtentions.Contains(fileExtention))
            {
                throw new BadRequestExceptionCustome(
                    $"صيغة الملف غير مسموح بها. الصيغ المدعومة هي: {string.Join(", ", AllowedExtentions)}");
            }

            // 3. فحص حجم الملف (File Size Limit)
            var fileSize = formFile.Length;
            if (fileSize > _fileSizeLimit)
            {
                // 👇 رسالة واضحة جداً لليوزر بالـ MB
                throw new BadRequestExceptionCustome(
                    $"حجم الملف كبير جداً. الحد الأقصى المسموح به هو ({_fileSizeLimit / (1024 * 1024)}) ميجابايت.");
            }

            // 4. Get Root Path (wwwroot/uploads) + SubFolder
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var folderPath = Path.Combine(webRootPath, subFolder);

            // 5. check if folderPath Exist
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 6. create unique file name
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtention}";

            // 7. create Full Path 
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            // 8. Save inside HARDDISK
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return Path.Combine("uploads", subFolder, uniqueFileName).Replace("\\", "/");
        }

        public async Task<bool> DeleteImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
    }
}