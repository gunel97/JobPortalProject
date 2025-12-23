using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IExperienceService:ICrudService<Experience, ExperienceViewModel,ExperienceCreateViewModel, ExperienceUpdateViewModel>
    {
        public Task<bool> AddExperienceToResume(ExperienceAddViewModel model, int resumeId);
    }
}
