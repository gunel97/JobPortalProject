using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using JobPortalProject.BL.ViewModels.Pagination;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobService : ICrudService<Job, JobViewModel, JobCreateViewModel, JobUpdateViewModel>
    {
        public Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId);
        public Task<JobCreateViewModel> GetJobCreateViewModelAsync(int companyId);
        public Task<bool> CreateJob(int companyId, JobCreateViewModel model);
        public Task<List<JobViewModel>> GetAllJobsOfCompanyAsync(int companyId);
        public Task<IEnumerable<JobViewModel>> GetAllJobsAsync();
        public Task<JobUpdateViewModel> GetUpdateViewModel(int jobId);
        public Task<bool> SoftDeleteJob(int id);
        public Task<bool> DeactivateJob(int id);
        public Task<PagedResultModel<JobViewModel>> GetPagedJobsAsync(int index = 0, int size = 10);
    }
}
