using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IAddressService : ICrudService<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
    {
    }
}
