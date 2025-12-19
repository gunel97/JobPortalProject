using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IPersonalInfoService:ICrudService<PersonalInfo, PersonalInfoViewModel, PersonalInfoCreateViewModel, PersonalInfoUpdateViewModel>
    {
        public Task<bool> AddAddressToPersonalInfo(int personalInfoId, Address address);
    }
}
