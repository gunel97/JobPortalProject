using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IResumeService:ICrudService<Resume, ResumeViewModel,ResumeCreateViewModel, ResumeUpdateViewModel>
    {
        public Task<Resume> GetResumeWithDetailsAsync(int resumeId, int languageId);
    }
}
