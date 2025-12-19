using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IPersonalInfoTranslationService : ICrudService<PersonalInfoTranslation, PersonalInfoTranslationViewModel, PersonalInfoTranslationCreateViewModel, PersonalInfoTranslationUpdateViewModel>
    {
    }
}
