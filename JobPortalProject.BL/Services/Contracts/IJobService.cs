using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobService : ICrudService<Job, JobViewModel, JobCreateViewModel, JobUpdateViewModel>
    {
        public Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId);
        public Task<JobCreateViewModel> GetJobCreateViewModelAsync(int companyId);
        public List<SelectListItem> GetJobTypeListItems();
        public List<SelectListItem> GetGenderListItems();
        public List<SelectListItem> GetSalaryTypeListItems();
        public List<SelectListItem> GetEducationTypeListItems();
        public Task<bool> CreateJob(int companyId, JobCreateViewModel model);
        public Task<List<JobViewModel>> GetAllJobsOfCompanyAsync(int companyId);
        public Task<IEnumerable<JobViewModel>> GetAllJobsAsync();
    }
}
