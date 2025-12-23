using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ProfileViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IPersonalInfoService : ICrudService<PersonalInfo, PersonalInfoViewModel, PersonalInfoCreateViewModel, PersonalInfoUpdateViewModel>
    {
        public Task<bool> AddAddressToPersonalInfo(int personalInfoId, Address address);
        public Task<PersonalInfoUpdateViewModel> GetPersonalInfoUpdateViewModel(int languageId);
        public Task<PersonalInfoCreateViewModel> GetPersonalInfoCreateViewModel(int languageId);
        public Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model);
        public Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model);
        public Task<ProfileTranslationCreateViewModel> GetProfileTranslationCreateViewModel(int languageId);
        public Task<bool> CreateProfileTranslation(int languageId, ProfileTranslationCreateViewModel model);
        public Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId);
        public Task<bool> UpdatePersonalInfo(PersonalInfoUpdateViewModel model);
        public Task<ProfileUpdateViewModel> GetProfileUpdateViewModel();
        public Task<bool> UpdateProfileTranslation(ProfileTranslationUpdateViewModel model);
        public Task<List<EducationUpdateViewModel>> GetEducationUpdateViewModel();
        public Task<List<ExperienceUpdateViewModel>> GetExperienceUpdateViewModel();
    }
}
