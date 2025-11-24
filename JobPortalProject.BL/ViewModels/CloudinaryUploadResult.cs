using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.DTOs
{
    public class CloudinaryUploadResult
    {
        public bool Success { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public string SecureUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}
