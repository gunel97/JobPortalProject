using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobTranslationManager : CrudManager<JobTranslation, JobTranslationViewModel, JobTranslationCreateViewModel, JobTranslationUpdateViewModel>
, IJobTranslationService
    {
        public JobTranslationManager(IRepositoryAsync<JobTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
