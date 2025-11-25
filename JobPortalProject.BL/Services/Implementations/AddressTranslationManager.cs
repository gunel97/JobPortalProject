using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class AddressTranslationManager : CrudManager<AddressTranslation, AddressTranslationViewModel, AddressTranslationCreateViewModel, AddressTranslationUpdateViewModel>
, IAddressTranslationService
    {
        public AddressTranslationManager(IRepositoryAsync<AddressTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
