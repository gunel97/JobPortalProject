using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobCategoryService : ICrudService<JobCategory, JobCategoryViewModel, JobCategoryCreateViewModel, JobCategoryUpdateViewModel>
    {
    }
}
