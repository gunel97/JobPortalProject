using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ExperienceManager:CrudManager<Experience, ExperienceViewModel, ExperienceCreateViewModel, ExperienceUpdateViewModel>
        , IExperienceService
    {
        public ExperienceManager(IRepositoryAsync<Experience> repository, IMapper mapper):base(repository, mapper) { }
    }
}
