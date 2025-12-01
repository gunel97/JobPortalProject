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

            var addresses = await _addressService.GetAllAsync();


            var company = await _companyService.GetAsync(
                                            predicate: x => !x.IsDeleted && x.Id == id,
                                            include: x => x
                                            .Include(ct => ct.CompanyTranslations!.Where(x => x.LanguageId == language.Id))
                                            .Include(x=>x.Addresses).ThenInclude(x=>x.AddressTranslations.Where(x=>x.LanguageId==language.Id))
                                        
                                            .Include(t => t.CompanyType).ThenInclude(ct => ct.CompanyTypeTranslations!.Where(x => x.LanguageId == language.Id))
                                            .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations.Where(x => x.LanguageId == language.Id)));

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var website = companySocials.FirstOrDefault(x => x.SocialMedia!.Title == "web");

            var companyDetailsViewModel = new CompanyDetailsViewModel
            {
                Company = company,
                CompanySocials = companySocials.ToList(),
                Website = website
            };

            return companyDetailsViewModel;

        }
    }
}
