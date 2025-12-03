using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CompanySocialViewModels
{
    public class CompanySocialViewModel
    {
        public int Id { get; set; }
        public int SocialMediaId { get; set; }
        public int CompanyId { get; set;}
        public string? AddressUrl { get; set; }
        public SocialMediaViewModel? SocialMedia { get; set; }
    }

    public class CompanySocialCreateViewModel
    {
        public int CompanyId { get; set; }
        public int SocialMediaId { get; set; }
        public string? AddressUrl { get; set; }
    }

    public class CompanySocialUpdateViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int SocialMediaId { get; set; }
        public string? AddressUrl { get; set; }
        public string? Title { get; set; }
        public string? IconUrl { get; set; }
    }
}
