using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobResponsibilityTranslationManager : CrudManager<JobResponsibilityTranslation, JobResponsibilityTranslationViewModel, JobResponsibilityTranslationCreateViewModel, JobResponsibilityTranslationUpdateViewModel>
, IJobResponsibilityTranslationService
    {
        public JobResponsibilityTranslationManager(IRepositoryAsync<JobResponsibilityTranslation> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<bool> AddTranslationToResponsibility(JobResponsibilityTranslationCreateViewModel model)
        {
            var translation = new JobResponsibilityTranslation
            {
                JobResponsibilityId = model.JobResponsibilityId,
                Value = model.Value,
                LanguageId = model.LanguageId,
            };

            var result = await Repository.AddAsync(translation);
            if (result == null)
                return false;

            return true;
        }
    }


}
