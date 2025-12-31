using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICountryService : ICrudService<Country, CountryViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetCountrySelectListItems();
        public Task<CountryDetailsViewModel> GetDetailsViewModel(int id);
        public Task<CountryUpdateViewModel> GetUpdateViewModel(int id);
        public Task<PagedResultModel<CountryViewModel>> GetPagedCountriesAsync(CountryFilterViewModel filter);
    }
}
