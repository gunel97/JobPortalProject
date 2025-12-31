using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class CountryIndexManager:ICountryIndexService
    {
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public CountryIndexManager(ICountryService countryService, ILanguageService languageService, ICookieService cookieService)
        {
            _countryService = countryService;
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<CountryPagedIndexViewModel> GetPagedCountryIndexModel(CountryFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new CountryFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedCountries = await _countryService.GetPagedCountriesAsync(filter);

            var model = new CountryPagedIndexViewModel
            {
                Languages = languages.ToList(),
                Filter = filter,
                Countries = pagedCountries
            };

            return model;
        }
    }
}
