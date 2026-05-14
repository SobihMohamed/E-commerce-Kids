using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Attachment
{
    public interface IAttachmentService
    {
        Task<string> UploadImageAsync(IFormFile? formFile, string folderName);
        public Task<bool> DeleteImage(string filePath);
    }
}
