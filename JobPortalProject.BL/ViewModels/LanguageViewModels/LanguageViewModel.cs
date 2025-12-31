using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.LanguageViewModels
{
    public class LanguageViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IsoCode { get; set; }
        public string? IconPublicId { get; set; } 
        public string? IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LanguageCreateViewModel
    {
        public string Name { get; set; } = null!;
        public IFormFile IconFile { get; set; }=null!;
        public string IsoCode { get; set; } = null!;
        public string? IconPublicId { get; set; }
        public string? IconUrl { get; set; }

    }

    public class LanguageUpdateViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IsoCode { get; set; }
        public string? IconPublicId { get; set; }
        public string? IconUrl { get; set; }
        public IFormFile? IconFile { get; set; }

    }
}
