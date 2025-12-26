using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IEducationService:ICrudService<Education, EducationViewModel, EducationCreateViewModel, EducationUpdateViewModel>
    {
        public Task<Education> Create(int resumeId, EducationCreateViewModel model);
        public Task<bool> AddEducationToResume(EducationAddViewModel model, int resumeId);
        public Task<EducationUpdatePageViewModel> GetEducationUpdateViewModel();
        public Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model);
        public Task<EducationPageCreateViewModel> GetEducationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model);
        public Task<EducationViewModel> GetEducationViewModel(int resumeId);
        public Task<List<EducationViewModel>> GetEducationModelsOfResume(int resumeId);
    }
}
