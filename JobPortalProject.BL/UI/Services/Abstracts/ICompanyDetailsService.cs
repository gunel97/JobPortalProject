using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface ICompanyDetailsService
    {
        Task<CompanyDetailsViewModel> GetCompanyDetailsAsync(int id);
    }
}
