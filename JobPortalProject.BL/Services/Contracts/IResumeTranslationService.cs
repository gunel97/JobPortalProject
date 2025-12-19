using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IResumeTranslationService:ICrudService<ResumeTranslation, ResumeTranslationViewModel,ResumeTranslationCreateViewModel, ResumeTranslationUpdateViewModel>
    {
        public Task<ResumeTranslationViewModel> Create(ResumeTranslationCreateViewModel model, int resumeId);
    }
}
