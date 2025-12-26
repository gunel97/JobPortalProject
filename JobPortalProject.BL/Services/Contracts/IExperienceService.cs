using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IExperienceService:ICrudService<Experience, ExperienceViewModel,ExperienceCreateViewModel, ExperienceUpdateViewModel>
    {
        public Task<bool> AddExperienceToResume(ExperienceAddViewModel model, int resumeId);
        public Task<ExperienceUpdatePageViewModel> GetExperienceUpdateViewModel();
        public Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId);
        public Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateExperienceTranslation(int languageId, ExperiencePageCreateViewModel model);
        public Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model);
        public Task<ExperienceViewModel> GetExperienceViewModel(int resumeId);
        public Task<List<ExperienceViewModel>> GetExperienceModelsOfResume(int resumeId);
    }
}
