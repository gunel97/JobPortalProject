using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IExperienceService:ICrudService<Experience, ExperienceViewModel,ExperienceCreateViewModel, ExperienceUpdateViewModel>
    {
    }
}
