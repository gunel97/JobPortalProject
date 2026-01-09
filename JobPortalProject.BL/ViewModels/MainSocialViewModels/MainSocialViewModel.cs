using Microsoft.AspNetCore.Http;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.MainSocialViewModels
{
    public class MainSocialViewModel
    {
        public int Id { get; set; }
        public string? IconUrl { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IFormFile? IconFile { get; set; }
    }

    public class MainSocialCreateViewModel
    {
        public required IFormFile IconFile { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
        public string? IconUrl { get; set; }
        public string? IconPublicId { get; set; }
    }

    public class MainSocialUpdateViewModel 
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? IconUrl { get; set; }
        public string? IconPublicId { get; set; }
        public IFormFile? IconFile { get; set; }

    }
}
