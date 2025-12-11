using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobExtraBenefitTranslationService : ICrudService<JobExtraBenefitTranslation, JobExtraBenefitTranslationViewModel, JobExtraBenefitTranslationCreateViewModel, JobExtraBenefitTranslationUpdateViewModel>
    {
    }
  
}
