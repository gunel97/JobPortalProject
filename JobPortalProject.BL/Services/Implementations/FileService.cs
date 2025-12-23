using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class FileService
    {

        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var contentType = file.ContentType.ToLowerInvariant();

            return contentType.StartsWith("image/");
        }
    }
}
