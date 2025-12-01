using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobService : ICrudService<Job, JobViewModel, JobCreateViewModel, JobUpdateViewModel>
    {
        public Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId);
    }
}
