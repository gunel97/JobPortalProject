using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobResponsibilityTranslationService : ICrudService<JobResponsibilityTranslation, JobResponsibilityTranslationViewModel, JobResponsibilityTranslationCreateViewModel, JobResponsibilityTranslationUpdateViewModel>
    {
    }
  
}
