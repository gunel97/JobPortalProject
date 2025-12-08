using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyService : ICrudService<Company, CompanyViewModel, CompanyCreateViewModel, CompanyUpdateViewModel>
    {
        public Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync();
        public Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync(int id);
        public Task<bool> IsActive(int companyId, int languageId);
        public Task<WorkingFieldCreateViewModel> GetWorkingFieldCreateViewModel(int selectedLanguageId);
        public Task<bool> CreateWorkingField(WorkingFieldCreateViewModel model);
        public Task<AddWorkingFieldTranslationViewModel> GetAddTranslationViewModelAsync(int selectedLanguageId);
    }
}
