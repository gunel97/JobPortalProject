using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICandidateService:ICrudService<Candidate, CandidateViewModel,CandidateCreateViewModel, CandidateUpdateViewModel>
    {
        public PersonalInfoCreateViewModel GetPersonalInfoCreateViewModel();
        public Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId);
        public Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model);
        public Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model);
    }
}
