using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class PersonalInfoTranslationManager : CrudManager<PersonalInfoTranslation, PersonalInfoTranslationViewModel, PersonalInfoTranslationCreateViewModel, PersonalInfoTranslationUpdateViewModel>
        , IPersonalInfoTranslationService
    {
        public PersonalInfoTranslationManager(IRepositoryAsync<PersonalInfoTranslation> repository, IMapper mapper):base(repository, mapper) { }
    }
}
