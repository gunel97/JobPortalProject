using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICityService : ICrudService<City, CityViewModel, CityCreateViewModel, CityUpdateViewModel>
    {
        public Task<CityUpdateViewModel> GetUpdateViewModel(int id);
        public Task<CityDetailsViewModel> GetDetailsViewModel(int id);
        public Task<PagedResultModel<CityViewModel>> GetPagedCitiesAsync(CityFilterViewModel filter);
        public Task<List<SelectListItem>> GetCitySelectListItemsWithCountry(int selectedLanguageId);
    }

}
