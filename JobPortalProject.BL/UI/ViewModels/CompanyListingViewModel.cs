using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.Pagination;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class PagedCompanyListingViewModel
    {
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<CompanyTypeViewModel> CompanyTypes { get; set; } = [];
        public PagedResultModel<CompanyViewModel> Companies { get; set; } = null!;
        public IEnumerable<IGrouping<string?, AddressViewModel>> AddressesCitiesGroup { get; set; } = [];
    }
    //public class CompanyListingViewModel
    //{
    //    public List<AddressViewModel> Addresses { get; set; } = [];
    //    public List<CompanyTypeViewModel> CompanyTypes { get; set; } = [];
    //    public List<CompanyViewModel> Companies { get; set; } = [];
    //    public IEnumerable<IGrouping<string?, AddressViewModel>> AddressesCitiesGroup { get; set; } = [];
    //}
}
