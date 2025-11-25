using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICountryTranslationService : ICrudService<CountryTranslation, CountryTranslationViewModel, CountryTranslationCreateViewModel, CountryTranslationUpdateViewModel>
    {
    }
}
