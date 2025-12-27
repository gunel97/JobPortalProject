using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyService : ICrudService<Company, CompanyViewModel, CompanyCreateViewModel, CompanyUpdateViewModel>
    {
        public Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync();
        public Task<CompanyTranslationEditPageViewModel> GetCompanyTranslationEditPageAsync(int languageId);
        public Task<bool> UpdateCompanyTranslation(CompanyTranslationEditPageViewModel model);
        public Task<bool> IsCompanyActive();
        public Task<int> GetCompanyIdOfUser();
        public Task<bool> AddTranslationToExistingCompany(AddTranslationToExistedCompanyViewModel model);
        public Task<AddTranslationToExistedCompanyViewModel> GetAddTranslationToExistedCompanyViewModel(int languageId);
        public Task<PagedResultModel<CompanyViewModel>> GetPagedCompaniesAsync(int index = 0, int size = 10);
    }
}
