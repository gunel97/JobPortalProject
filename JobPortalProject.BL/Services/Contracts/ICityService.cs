using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICityService : ICrudService<City, CityViewModel, CityCreateViewModel, CityUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetCitySelectListItemsWithCountry(int selectedLanguageId);
    }
}
