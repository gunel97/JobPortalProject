using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICandidateService : ICrudService<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
    {
        public Task<PersonalInfoCreateViewModel> GetPersonalInfoCreateViewModel(int languageId);
        public Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId);
        public Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model);
        public Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model);
        public Task<EducationPageCreateViewModel> GetEducationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model);
        public Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model);
        public Task<CandidateDashboardViewModel> GetDashboardViewModel();
        public Task<ProfileTranslationCreateViewModel> GetProfileTranslationCreateViewModel(int languageId);
        public Task<bool> CreateProfileTranslation(int languageId, ProfileTranslationCreateViewModel model);
        public Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model);
        public Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId);
        public Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId);
        public Task<bool> CreateExperienceTranslation(int languageId, ExperiencePageCreateViewModel model);
        public Task<PersonalInfoUpdateViewModel> GetPersonalInfoUpdateViewModel(int languageId);
    }
}
