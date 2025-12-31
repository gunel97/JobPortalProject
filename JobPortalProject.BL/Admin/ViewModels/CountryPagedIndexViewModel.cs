using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class CountryPagedIndexViewModel
    {
        public CountryFilterViewModel? Filter { get; set; }
        public PagedResultModel<CountryViewModel> Countries { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
    }
}
