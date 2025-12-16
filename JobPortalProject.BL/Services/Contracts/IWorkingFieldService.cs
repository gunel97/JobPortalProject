using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IWorkingFieldService : ICrudService<WorkingField, WorkingFieldViewModel, WorkingFieldCreateViewModel, WorkingFieldUpdateViewModel>
    {
        public Task<List<WorkingFieldUpdateViewModel>> GetUpdateViewModelAsync(int companyId, int languageId);
        public  Task<List<SelectListItem>> GetWorkingFieldSelectListItemsAsync(int companyId, int selectedLanguageId);
        public Task<WorkingFieldTranslationViewModel> CreateWorkingFieldTranslationAsync(AddWorkingFieldTranslationViewModel model);
        public Task<bool> UpdateWorkingFieldAsync(int languageId, int workingFieldId, WorkingFieldUpdateViewModel model);
        public Task<bool> CreateWorkingField(int companyId, WorkingFieldCreateViewModel model);
        public Task<bool> AddTranslationToExistingWorkingField(WorkingFieldTranslationCreateViewModel model);
    }
}
