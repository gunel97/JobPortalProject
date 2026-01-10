using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class IndexViewModel
    {
        public List<OrderIndexViewModel> Orders { get; set; } = [];
        public int NewCandidateCount { get; set; }
        public int NewCompanyCount { get; set; }
        public List<LanguageViewModel> Languages { get; set; } = [];
    }
}
