using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
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

        public async Task<PagedCompanyListingViewModel> GetListsAsync(CompanyFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();

            var addresses = await _addressService.GetAllAsync(predicate: x=>x.Company!=null);

            var addressesByCities = addresses.DistinctBy(x => x.CityName).ToList();
            var addressesCitiesGroup = addresses.GroupBy(a => a.CityName).ToList();

            var pagedCompanies = await _companyService.GetPagedCompaniesAsync(filter);

            var companyTypes = await _companyTypeService.GetAllAsync(
                                        predicate: x => !x.IsDeleted && x.CompanyTypeTranslations.Any() && x.Companies.Count!=0,
                                        include: x => x
                                        .Include(c => c.CompanyTypeTranslations.Where(ct => ct.LanguageId == language.Id))
                                        .Include(c=>c.Companies));

            var companyListingViewModel = new PagedCompanyListingViewModel
            {
                Companies = pagedCompanies,
                CompanyTypes = companyTypes.ToList(),
                AddressesCitiesGroup=addressesCitiesGroup,
                Filter = filter
            };

            return companyListingViewModel;
        }
    }
}
