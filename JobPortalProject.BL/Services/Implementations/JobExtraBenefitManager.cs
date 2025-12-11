using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobExtraBenefitManager : CrudManager<JobExtraBenefit, JobExtraBenefitViewModel, JobExtraBenefitCreateViewModel, JobExtraBenefitUpdateViewModel>
, IJobExtraBenefitService
    {
        public JobExtraBenefitManager(IRepositoryAsync<JobExtraBenefit> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<bool> CreateJobBenefitAsync(JobExtraBenefitCreateViewModel createViewModel)
        {
            var jobBenefit = new JobExtraBenefit();
            jobBenefit.JobId = createViewModel.JobId;
            jobBenefit.JobExtraBenefitTranslations = createViewModel.Translations.Select(
                x => new JobExtraBenefitTranslation
                {
                    Value = x.Value,
                    LanguageId = x.LanguageId,
                }).ToList();

            var created = await Repository.AddAsync(jobBenefit);
            if (created == null)
            {
                return false;
            }

            return true;
        }
    }


}
