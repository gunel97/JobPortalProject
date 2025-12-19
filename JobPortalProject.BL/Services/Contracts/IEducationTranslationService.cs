using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IEducationTranslationService:ICrudService<EducationTranslation, EducationTranslationViewModel, EducationTranslationCreateViewModel, EducationTranslationUpdateViewModel>
    {
    }
}
