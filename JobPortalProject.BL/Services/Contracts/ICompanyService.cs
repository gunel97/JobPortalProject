using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyService : ICrudService<Company, CompanyViewModel, CompanyCreateViewModel, CompanyUpdateViewModel>
    {
        public Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync();
        public Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync();
        public Task<CompanyTranslationEditPageViewModel> GetCompanyTranslationEditPageAsync(int languageId);
        public Task<bool> UpdateCompanyTranslation(CompanyTranslationEditPageViewModel model);
        public Task<WorkingFieldCreateViewModel> GetWorkingFieldCreateViewModel();
        public Task<bool> CreateWorkingField(WorkingFieldCreateViewModel model);
        public Task<AddressCreateViewModel> GetAddressCreateViewModel();
        public Task<bool> CreateAddress(AddressCreateViewModel model);
        public Task<bool> IsCompanyActive();
    }
}
