using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICandidateService : ICrudService<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
    {
        public Task<CandidateDashboardViewModel> GetDashboardViewModel();
        public Task<Candidate> GetCandidate();
        public Task<Candidate> GetCandidateWithTranslation(int languageId);
        
    }
}
