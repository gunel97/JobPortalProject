using JobPortalProject.BL.UI.ViewModels;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface ICompanyListingService
    {
        Task<CompanyListingViewModel> GetListsAsync();
    }
}
