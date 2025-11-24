using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobCategoryManager : CrudManager<JobCategory, JobCategoryViewModel, JobCategoryCreateViewModel, JobCategoryUpdateViewModel>
    , IJobCategoryService
    {
        public JobCategoryManager(IRepositoryAsync<JobCategory> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
