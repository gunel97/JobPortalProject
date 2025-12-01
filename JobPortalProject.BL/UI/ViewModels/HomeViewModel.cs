using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class HomeViewModel
    {
        public List<JobCategoryViewModel> JobCategories { get; set; } = [];
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<CompanyViewModel> Companies { get; set; } = [];
        public List<JobViewModel> Jobs {  get; set; } = [];
    }
}
