using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class CompanyDetailsViewModel
    {
        public CompanyViewModel? Company { get; set; }
        public List<CompanySocialViewModel>? CompanySocials { get; set; } = [];
        public CompanySocialViewModel? Website { get; set; }
    }
}
