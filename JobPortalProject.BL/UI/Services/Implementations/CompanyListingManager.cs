using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class CompanyListingManager : ICompanyListingService
    {
        private readonly IAddressService _addressService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICookieService _cookieService;
        private readonly ICityService _cityService;


        public CompanyListingManager(IAddressService addressService, ICompanyService companyService, ICompanyTypeService companyTypeService, ICookieService cookieService, ICityService cityService)
        {
            _addressService = addressService;
            _companyService = companyService;
            _companyTypeService = companyTypeService;
            _cookieService = cookieService;
            _cityService = cityService;
        }

        public async Task<CompanyListingViewModel> GetListsAsync()
        {
            var language = await _cookieService.GetLanguageAsync();

            var cities = await _cityService.GetAllAsync(
                                        predicate: x => !x.IsDeleted,
                                        include: c => c
                                        .Include(ct => ct.CityTranslations!
                                        .Where(c => c.LanguageId == language.Id))
                                        .Include(c => c.Country!)
                                        .ThenInclude(ct => ct.Translations!.Where(x => x.LanguageId == language.Id)));

            var companies = await _companyService.GetAllAsync(
                                        predicate: x => !x.IsDeleted,
                                        include: c => c
                                        .Include(ct => ct.CompanyTranslations!
                                        .Where(c => c.LanguageId == language.Id))
                                        .Include(c=>c.CompanyAddresses!.Where(x=>x.IsMain))
                                        .ThenInclude(x=>x.Address!)
                                        .ThenInclude(x=>x.AddressTranslations!.Where(x=>x.LanguageId==language.Id)!));

            var companyTypes =await _companyTypeService.GetAllAsync(
                                        predicate: x => !x.IsDeleted,
                                        include:x=>x
                                        .Include(c=>c.CompanyTypeTranslations.Where(ct=>ct.LanguageId== language.Id)));

            var companyListingViewModel = new CompanyListingViewModel
            {
                Cities = cities.ToList(),
                Companies=companies.ToList(),
                CompanyTypes=companyTypes.ToList()
            };

            return companyListingViewModel;
        }
    }
}
