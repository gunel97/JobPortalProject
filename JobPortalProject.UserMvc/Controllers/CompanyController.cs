using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.UserMvc.Controllers
{
    public class CompanyController: Controller
    {
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly ICompanyListingService _companyListingService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyDashboardService _companyDashboardService;
        private readonly ICookieService _cookieService;

        public CompanyController(ICompanyDetailsService companyDetailsService, ICompanyListingService companyListingService, ICompanyService companyService, ICompanyDashboardService companyDashboardService, ICookieService cookieService)
        {
            _companyDetailsService = companyDetailsService;
            _companyListingService = companyListingService;
            _companyService = companyService;
            _companyDashboardService = companyDashboardService;
            _cookieService = cookieService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _companyListingService.GetListsAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            int companyId = int.Parse(id.Split('-').Last());

            var companyDetailsViewModel = await _companyDetailsService.GetCompanyDetailsAsync(companyId);

            if (companyDetailsViewModel.Company == null)
                return NotFound();

            return View(companyDetailsViewModel);
        }

        public async Task<IActionResult> CompanyDashboard()
        {
            var model = await _companyDashboardService.GetCompanyDashboardViewModelAsync();

            return View(model);
        }

        public async Task<IActionResult> EditCompanyProfile(int id)
        {
            var language = await _cookieService.GetLanguageAsync();
            var model = await _companyService.GetCompanyUpdateViewModelAsync(language.Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(int id, CompanyUpdateViewModel model)
        {
            var language = await _cookieService.GetLanguageAsync();
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetCompanyUpdateViewModelAsync(language.Id);

                return View(model);
            }

            var isUpdated = await _companyService.UpdateAsync(1, model);
          
            if (!isUpdated)
                return NotFound();

            return RedirectToAction("CompanyDashboard", "Company");
        }
    }
}
