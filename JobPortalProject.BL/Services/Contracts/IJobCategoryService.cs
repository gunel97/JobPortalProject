using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobCategoryService : ICrudService<JobCategory, JobCategoryViewModel, JobCategoryCreateViewModel, JobCategoryUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetJobCategorySelectListItems(int selectedLanguageId);
        public Task<JobCategoryViewModel> CreateJobCategoryAsync(JobCategoryCreateViewModel model);
        public Task<JobCategoryUpdateViewModel> GetUpdateViewModel(int id);
        public Task<bool> UpdateJobCategoryAsync(JobCategoryUpdateViewModel model);
        public Task<PagedResultModel<JobCategoryViewModel>> GetPagedJobCategoriesAsync(JobCategoryFilterViewModel filter);
    }
}
