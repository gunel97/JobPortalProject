using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobResponsibilityManager : CrudManager<JobResponsibility, JobResponsibilityViewModel, JobResponsibilityCreateViewModel, JobResponsibilityUpdateViewModel>
, IJobResponsibilityService
    {
        public JobResponsibilityManager(IRepositoryAsync<JobResponsibility> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<bool> CreateJobResponsibilityAsync(JobResponsibilityCreateViewModel createViewModel)
        {
            var jobResponsibility = new JobResponsibility();
            jobResponsibility.JobId= createViewModel.JobId;
            jobResponsibility.JobResponsibilityTranslations=createViewModel.JobResponsibilityTranslations.Select(
                x=> new JobResponsibilityTranslation
            {
                    Value=x.Value,
                    LanguageId=x.LanguageId,
            }).ToList();

             var created = await Repository.AddAsync(jobResponsibility);
            if (created == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddResponsibilityToJob(JobResponsibilityCreateViewModel model)
        {
            return true;
        }
    }


}
