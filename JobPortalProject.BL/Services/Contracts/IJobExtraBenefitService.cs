using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobExtraBenefitService : ICrudService<JobExtraBenefit, JobExtraBenefitViewModel, JobExtraBenefitCreateViewModel, JobExtraBenefitUpdateViewModel>
    {
        public Task<bool> CreateJobBenefitAsync(JobExtraBenefitCreateViewModel createViewModel);
    }
  
}
