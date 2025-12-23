using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICandidateService : ICrudService<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
    {
        public Task<EducationPageCreateViewModel> GetEducationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model);
        public Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model);
        public Task<CandidateDashboardViewModel> GetDashboardViewModel();
        public Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model);
        public Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId);
        public Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateExperienceTranslation(int languageId, ExperiencePageCreateViewModel model);
        public Task<Candidate> GetCandidate();
        public Task<Candidate> GetCandidateWithTranslation(int languageId);
    }
}
