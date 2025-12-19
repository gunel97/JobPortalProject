using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IExperienceTranslationService:ICrudService<ExperienceTranslation, ExperienceTranslationViewModel, ExperienceTranslationCreateViewModel, ExperienceTranslationUpdateViewModel>
    {
    }
}
