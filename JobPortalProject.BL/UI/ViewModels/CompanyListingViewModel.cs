using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class CompanyListingViewModel
    {
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<CompanyTypeViewModel> CompanyTypes { get; set; } = [];
        public List<CompanyViewModel> Companies { get; set; } = [];
        public IEnumerable<IGrouping<string?, AddressViewModel>> AddressesCitiesGroup { get; set; } = [];
    }
}
