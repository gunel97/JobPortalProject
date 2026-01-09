using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.BioViewModels
{
    public class BioViewModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BioCreateViewModel
    {
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public IFormFile LogoFile { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? LogoPublicId { get; set; }
    }

    public class BioUpdateViewModel
    {
        public int Id { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? LogoUrl { get; set; }
        public string? LogoPublicId { get; set; }
        public IFormFile? LogoFile { get; set; }
    }
}
