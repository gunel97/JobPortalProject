using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobResponsibilityTranslationManager : CrudManager<JobResponsibilityTranslation, JobResponsibilityTranslationViewModel, JobResponsibilityTranslationCreateViewModel, JobResponsibilityTranslationUpdateViewModel>
, IJobResponsibilityTranslationService
    {
        public JobResponsibilityTranslationManager(IRepositoryAsync<JobResponsibilityTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
