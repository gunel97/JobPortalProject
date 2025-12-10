using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobCategoryService : ICrudService<JobCategory, JobCategoryViewModel, JobCategoryCreateViewModel, JobCategoryUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetJobCategorySelectListItems(int selectedLanguageId);
    }
}
