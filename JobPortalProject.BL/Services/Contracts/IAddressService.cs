using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    
    public interface IAddressService : ICrudService<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
    {
        public Task<List<AddressViewModel>> GetCompaniesAddressesAsync(int languageId);
        public Task<List<AddressUpdateViewModel>> GetAddressUpdateViewModels(int companyId, int selectedLanguageId);
        public Task<bool> UpdateAddressAsync(int languageId, int addressId, AddressUpdateViewModel model);
        public Task<List<SelectListItem>> GetAddressSelectListItems(int companyId, int languageId);
        public Task<List<Address>> GetByCompanyIdAsync(int companyId);
        public Task<bool> CreateAddress(int companyId, AddressCreateViewModel model);
        public Task<bool> AddTranslationToExistingAddress(AddressTranslationCreateViewModel model);

    }
}
