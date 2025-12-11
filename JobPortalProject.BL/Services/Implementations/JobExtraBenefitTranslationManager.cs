using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobExtraBenefitTranslationManager : CrudManager<JobExtraBenefitTranslation, JobExtraBenefitTranslationViewModel, JobExtraBenefitTranslationCreateViewModel, JobExtraBenefitTranslationUpdateViewModel>
, IJobExtraBenefitTranslationService
    {
        public JobExtraBenefitTranslationManager(IRepositoryAsync<JobExtraBenefitTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
