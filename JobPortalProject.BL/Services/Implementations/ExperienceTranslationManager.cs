using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ExperienceTranslationManager : CrudManager<ExperienceTranslation, ExperienceTranslationViewModel, ExperienceTranslationCreateViewModel, ExperienceTranslationUpdateViewModel>
        , IExperienceTranslationService
    {
        public ExperienceTranslationManager(IRepositoryAsync<ExperienceTranslation> repository, IMapper mapper):base(repository, mapper) { }
    }
}
