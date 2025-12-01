using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobTranslationService : ICrudService<JobTranslation, JobTranslationViewModel, JobTranslationCreateViewModel, JobTranslationUpdateViewModel>
    {
    }
}
