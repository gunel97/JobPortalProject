using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobResponsibilityService : ICrudService<JobResponsibility, JobResponsibilityViewModel, JobResponsibilityCreateViewModel, JobResponsibilityUpdateViewModel>
    {
        public Task<bool> CreateJobResponsibilityAsync(JobResponsibilityCreateViewModel createViewModel);
    }
  
}
