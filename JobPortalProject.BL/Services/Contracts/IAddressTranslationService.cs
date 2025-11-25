using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IAddressTranslationService : ICrudService<AddressTranslation, AddressTranslationViewModel, AddressTranslationCreateViewModel, AddressTranslationUpdateViewModel>
    {
    }
}
