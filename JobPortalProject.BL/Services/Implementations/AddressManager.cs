using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class AddressManager : CrudManager<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
, IAddressService
    {
        public AddressManager(IRepositoryAsync<Address> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
