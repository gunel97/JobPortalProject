using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyService : ICrudService<Company, CompanyViewModel, CompanyCreateViewModel, CompanyUpdateViewModel>
    {
        public Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync();
        public Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync(int id);
    }
}
