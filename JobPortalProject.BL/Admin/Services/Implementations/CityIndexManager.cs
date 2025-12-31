using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class CityIndexManager:ICityIndexService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CityIndexManager(ICookieService cookieService, ILanguageService languageService, ICityService cityService, ICountryService countryService)
        {
            _cookieService = cookieService;
            _languageService = languageService;
            _cityService = cityService;
            _countryService = countryService;
        }

        public async Task<CityPagedIndexViewModel> GetPagedCityIndexModel(CityFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new CityFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var countries = await _countryService.GetCountrySelectListItems();
            var pagedCities = await _cityService.GetPagedCitiesAsync(filter);

            var model = new CityPagedIndexViewModel
            {
                Languages = languages.ToList(),
                Filter = filter,
                Cities = pagedCities,
                Countries=countries
            };

            return model;
        }

    }
}
