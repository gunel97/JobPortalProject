using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class CompanyTypePagedIndexViewModel
    {
        public CompanyTypeFilterViewModel? Filter { get; set; }
        public PagedResultModel<CompanyTypeViewModel> CompanyTypes { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
    }


}
