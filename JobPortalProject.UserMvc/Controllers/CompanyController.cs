using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.UserMvc.Controllers
{
    public class CompanyController: Controller
    {
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly ICompanyListingService _companyListingService;

        public CompanyController(ICompanyDetailsService companyDetailsService, ICompanyListingService companyListingService)
        {
            _companyDetailsService = companyDetailsService;
            _companyListingService = companyListingService;
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
    }
}
