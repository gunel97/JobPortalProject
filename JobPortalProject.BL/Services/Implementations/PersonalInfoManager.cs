using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class PersonalInfoManager:CrudManager<PersonalInfo, PersonalInfoViewModel, PersonalInfoCreateViewModel, PersonalInfoUpdateViewModel>
        , IPersonalInfoService
    {
        public PersonalInfoManager(IRepositoryAsync<PersonalInfo> repository, IMapper mapper):base(repository, mapper) { }

        public async Task<bool> AddAddressToPersonalInfo(int personalInfoId, Address address)
        {
            var personalInfo = await Repository.GetByIdAsync(personalInfoId);
            if (personalInfo == null)
                return false;
            personalInfo.Address = address;
            var result = await Repository.UpdateAsync(personalInfo);
            if(result==null) return false;
            return true;
        }
    }
}
