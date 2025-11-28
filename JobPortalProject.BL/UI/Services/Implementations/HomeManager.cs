using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class HomeManager:IHomeService
    {
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICookieService _cookieService;
        private readonly IAddressService _addressService;
        private readonly ICompanyService _companyService;

        public HomeManager(IJobCategoryService jobCategoryService, ICookieService cookieService, IAddressService addressService, ICompanyService companyService)
        {
            _jobCategoryService = jobCategoryService;
            _cookieService = cookieService;
            _addressService = addressService;
            _companyService = companyService;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();

            var jobCategories = await _jobCategoryService.GetAllAsync(
                                                predicate: x => !x.IsDeleted,
                                                include: c => c
                                               .Include(ct => ct.JobCategoryTranslations!
                                               .Where(j => j.LanguageId == language.Id)));

            var addresses = await _addressService.GetAllAsync(
                                            //predicate: x => !x.IsDeleted && x.CompanyAddresses.Any(),
                                            include: x => x
                                            .Include(at => at.AddressTranslations!.Where(at => at.LanguageId == language.Id))
                                            .Include(a => a.City!).ThenInclude(c => c.CityTranslations!.Where(a => a.LanguageId == language.Id))
                                            .Include(a => a.City!).ThenInclude(c => c.Country!).ThenInclude(ct => ct.Translations!
                                            .Where(a => a.LanguageId == language.Id)));

            var addressesByCities = addresses.DistinctBy(a => a.City!.Name!);


            var companies = await _companyService.GetAllAsync(
                                               include: c => c
                                               .Include(ct => ct.CompanyTranslations!
                                               .Where(c => c.LanguageId == language.Id)));


            var homeViewModel = new HomeViewModel
            {
                JobCategories = jobCategories.ToList(),
                Addresses = addressesByCities.ToList(),
                Companies = companies.ToList(),
            };

            return homeViewModel;
        }


    }
}
