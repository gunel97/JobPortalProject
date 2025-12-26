using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IResumeService:ICrudService<Resume, ResumeViewModel,ResumeCreateViewModel, ResumeUpdateViewModel>
    {
        public Task<ResumeViewModel> GetResumeBase(int id);
        public Task<Resume> GetResumeWithDetailsAsync(int resumeId, int languageId);
        public Task<ResumeViewModel> CreateResume(int candidateId);
        public Task<ResumeViewModel> GetResume(int resumeId);
    }
}
