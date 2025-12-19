using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class EducationTranslationManager:CrudManager<EducationTranslation, EducationTranslationViewModel, EducationTranslationCreateViewModel, EducationTranslationUpdateViewModel>
        , IEducationTranslationService
    {
        public EducationTranslationManager(IRepositoryAsync<EducationTranslation> repository, IMapper mapper):base(repository, mapper) { }
    }
}
