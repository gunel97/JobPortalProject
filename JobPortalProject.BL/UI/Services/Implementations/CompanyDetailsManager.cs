using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class CompanyDetailsManager : ICompanyDetailsService
    {
        private readonly ICookieService _cookieService;
        private readonly ICompanyService _companyService;
        private readonly IAddressService _addressService;
        private readonly ICompanySocialService _companySocialService;
        public CompanyDetailsManager(ICompanyService companyService, ICookieService cookieService, IAddressService addressService, ICompanySocialService companySocialService)
        {
            _companyService = companyService;
            _cookieService = cookieService;
            _addressService = addressService;
            _companySocialService = companySocialService;
        }

        public async Task<CompanyDetailsViewModel> GetCompanyDetailsAsync(int id)
        {
            var language = await _cookieService.GetLanguageAsync();

            var companyAddresses = await _addressService.GetAllAsync(
                                          predicate: x => !x.IsDeleted && x.CompanyAddresses.Any(c=>c.CompanyId==id),
                                          include: x => x
                                          .Include(ca => ca.CompanyAddresses)
                                          .Include(at => at.AddressTranslations!.Where(at => at.LanguageId == language.Id))
                                          .Include(a => a.City!).ThenInclude(c => c.CityTranslations!.Where(a => a.LanguageId == language.Id))
                                          .Include(a => a.City!).ThenInclude(c => c.Country!).ThenInclude(ct => ct.Translations!
                                          .Where(a => a.LanguageId == language.Id)));

            var company = await _companyService.GetAsync(
                                            predicate: x => !x.IsDeleted && x.Id == id,
                                            include: x => x
                                            .Include(ct => ct.CompanyTranslations!.Where(x => x.LanguageId == language.Id))
                                            .Include(t=>t.CompanyType).ThenInclude(ct=>ct.CompanyTypeTranslations!.Where(x=>x.LanguageId==language.Id))
                                            .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations.Where(x => x.LanguageId == language.Id)));

            var mainAddress = await _addressService.GetAsync(
                                          predicate: x => !x.IsDeleted && x.CompanyAddresses.Any(c => c.CompanyId == id && c.IsMain)                                          ,
                                          include: x => x
                                          .Include(ca => ca.CompanyAddresses)
                                          .Include(at => at.AddressTranslations!.Where(at => at.LanguageId == language.Id))
                                          .Include(a => a.City!).ThenInclude(c => c.CityTranslations!.Where(a => a.LanguageId == language.Id))
                                          .Include(a => a.City!).ThenInclude(c => c.Country!).ThenInclude(ct => ct.Translations!
                                          .Where(a => a.LanguageId == language.Id)));

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var website = companySocials.FirstOrDefault(x => x.SocialMedia!.Title == "web");

            var companyDetailsViewModel = new CompanyDetailsViewModel
            {
                Company = company,
                CompanyAddresses = companyAddresses.ToList(),
                MainAddress = mainAddress,
                CompanySocials = companySocials.ToList(),
                Website = website
            };

            return companyDetailsViewModel;
                
        }
    }
}
