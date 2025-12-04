using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using JobPortalProject.BL.DTOs;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CloudinaryManager : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;

        public CloudinaryManager(IOptions<CloudinarySettings> settings)
        {
            _settings = settings.Value;

            var account = new Account(
                _settings.CloudName,
                _settings.ApiKey,
                _settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<CloudinaryUploadResult> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length !< 0)
            {
                return new CloudinaryUploadResult
                {
                    Success = false,
                    ErrorMessage = "File is empty"
                };
            }

            if (!file.ContentType.ToLowerInvariant().StartsWith("image/"))
            {
                return new CloudinaryUploadResult
                {
                    Success = false,
                    ErrorMessage = "File is not an image"
                };
            }

            string fileName = string.Concat(Guid.NewGuid(), file.FileName.Substring(file.FileName.LastIndexOf('.')));

            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Folder=folderName,
                    Transformation = new Transformation()
                        .Quality("auto:good")
                        .FetchFormat("auto"),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new CloudinaryUploadResult
                    {
                        Success = true,
                        PublicId = uploadResult.PublicId,
                        Url = uploadResult.Url.ToString(),
                        SecureUrl = uploadResult.SecureUrl.ToString()
                    };
                }

                return new CloudinaryUploadResult
                {
                    Success = false,
                    ErrorMessage = uploadResult.Error?.Message ?? "Upload failed"
                };
            }
            catch (Exception ex)
            {
                return new CloudinaryUploadResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return false;

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}
