using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CityManager : CrudManager<City, CityViewModel, CityCreateViewModel, CityUpdateViewModel>
, ICityService
    {
        public CityManager(IRepositoryAsync<City> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<List<SelectListItem>> GetCitySelectListItemsWithCountry(int selectedLanguageId)
        {
            var citySelectListItems = new List<SelectListItem>();
            var cities = await Repository.GetAllAsync(include: x => x.
                Include(c => c.CityTranslations.Where(t => t.LanguageId == selectedLanguageId)).
                Include(c => c.Country).ThenInclude(ct => ct.Translations.Where(t => t.LanguageId == selectedLanguageId)));                

            var cityViewModels = cities.Select(
                x => Mapper.Map<CityViewModel>(x)).ToList();
            cityViewModels.ForEach(x => citySelectListItems.Add(
                new SelectListItem(x.Name + ", "+ x.Country!.Name, x.Id.ToString())));

            return citySelectListItems;
        }
    }


}
