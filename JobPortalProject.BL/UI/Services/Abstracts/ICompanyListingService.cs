using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface ICompanyListingService
    {
        Task<PagedCompanyListingViewModel> GetListsAsync(CompanyFilterViewModel filter);
    }
}
