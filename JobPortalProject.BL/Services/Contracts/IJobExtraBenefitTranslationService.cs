using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobExtraBenefitTranslationService : ICrudService<JobExtraBenefitTranslation, JobExtraBenefitTranslationViewModel, JobExtraBenefitTranslationCreateViewModel, JobExtraBenefitTranslationUpdateViewModel>
    {
        public Task<bool> AddTranslationToBenefit(JobExtraBenefitTranslationCreateViewModel model);
    }
  
}
