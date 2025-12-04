using JobPortalProject.BL.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folderName);
        public Task<bool> DeleteImageAsync(string publicId);
    }
}
