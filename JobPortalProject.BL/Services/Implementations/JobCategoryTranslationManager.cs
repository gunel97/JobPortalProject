using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobCategoryTranslationManager : CrudManager<JobCategoryTranslation, JobCategoryTranslationViewModel, JobCategoryTranslationCreateViewModel, JobCategoryTranslationUpdateViewModel>
, IJobCategoryTranslationService
    {
        public JobCategoryTranslationManager(IRepositoryAsync<JobCategoryTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
