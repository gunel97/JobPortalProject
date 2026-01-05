using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobService : ICrudService<Job, JobViewModel, JobCreateViewModel, JobUpdateViewModel>
    {
        public Task<bool> CheckHasExpired(int jobId);
        public Task<PagedJobsOfCompanyViewModel> GetPagedJobsOfCompanyModel(JobFilterViewModel filter, int companyId);
        public Task<List<JobViewModel>> GetActiveJobsOfCompanyAsync(int companyId);
        public Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId);
        public Task<JobCreateViewModel> GetJobCreateViewModelAsync(int companyId);
        public Task<bool> CreateJob(int companyId, JobCreateViewModel model);
        public Task<IEnumerable<JobViewModel>> GetAllJobsAsync();
        public Task<JobUpdateViewModel> GetUpdateViewModel(int jobId);
        public Task<bool> SoftDeleteJob(int id);
        public Task<bool> DeactivateJob(int id);
        public Task<PagedResultModel<JobViewModel>> GetPagedJobsAsync(JobFilterViewModel filter);
        public Task<Dictionary<int, int>> GetJobCountJobType();
        public Task<Dictionary<int, int>> GetJobCountGender();
    }
}
