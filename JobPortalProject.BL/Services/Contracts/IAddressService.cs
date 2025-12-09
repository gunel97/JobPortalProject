using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    
    public interface IAddressService : ICrudService<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
    {
        public Task<List<AddressUpdateViewModel>> GetAddressUpdateViewModels(int companyId, int selectedLanguageId);
        public Task<bool> UpdateAddressAsync(int languageId, int addressId, AddressUpdateViewModel model);

    }
}
