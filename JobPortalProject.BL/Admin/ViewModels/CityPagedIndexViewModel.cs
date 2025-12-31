using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class CityPagedIndexViewModel
    {
        public CityFilterViewModel? Filter { get; set; }
        public PagedResultModel<CityViewModel> Cities { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
        public List<SelectListItem> Countries { get; set; } = [];
    }
}
