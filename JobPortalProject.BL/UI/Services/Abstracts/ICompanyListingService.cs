using JobPortalProject.BL.UI.ViewModels;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface ICompanyListingService
    {
        Task<PagedCompanyListingViewModel> GetListsAsync(int index=0, int size=10);
    }
}
