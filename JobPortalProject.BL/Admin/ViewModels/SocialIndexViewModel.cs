using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.MainSocialViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class SocialIndexViewModel
    {
        public List<MainSocialViewModel> Socials { get; set; } = [];
        public List<LanguageViewModel> Languages { get; set; } = []; 
    }
}
