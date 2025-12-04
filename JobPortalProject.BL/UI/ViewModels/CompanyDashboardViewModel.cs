using JobPortalProject.BL.ViewModels.LanguageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class CompanyDashboardViewModel
    {
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsAccountActive { get; set; }
        public List<LanguageViewModel> Languages { get; set; } = [];
    }
}
