using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICountryService : ICrudService<Country, CountryViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    {
    }
}
