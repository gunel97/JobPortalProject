using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.Controllers
{
    public class CompanyController: Controller
    {
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly ICompanyListingService _companyListingService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyDashboardService _companyDashboardService;
        private readonly ICookieService _cookieService;
        private readonly IWorkingFieldService _workingFieldService;
        private readonly IWorkingFieldTranslationService _workingFieldTranslationService;
        private readonly IAddressService _addressService;

        public CompanyController(ICompanyDetailsService companyDetailsService, ICompanyListingService companyListingService, 
            ICompanyService companyService, ICompanyDashboardService companyDashboardService, ICookieService cookieService, 
            IWorkingFieldService workingFieldService, IWorkingFieldTranslationService workingFieldTranslationService, 
            IAddressService addressService)
        {
            _companyDetailsService = companyDetailsService;
            _companyListingService = companyListingService;
            _companyService = companyService;
            _companyDashboardService = companyDashboardService;
            _cookieService = cookieService;
            _workingFieldService = workingFieldService;
            _workingFieldTranslationService = workingFieldTranslationService;
            _addressService = addressService;
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
            var isActive = await _companyService.IsCompanyActive();
            model.IsAccountActive = isActive;

            return View(model);
        }

        public async Task<IActionResult> EditCompanyProfile()
        {
            var language = await _cookieService.GetLanguageAsync();
            var model = await _companyService.GetCompanyUpdateViewModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(int id, CompanyUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetCompanyUpdateViewModelAsync();

                return View(model);
            }

            var isUpdated = await _companyService.UpdateAsync(model.SelectedUpdateLanguageId, model);

            if (!isUpdated)
                return NotFound();

            return RedirectToAction("CompanyDashboard", "Company");
        }

        public async Task<IActionResult> UpdateCompanyTranslation(int id)
        {
            var model = await _companyService.GetCompanyTranslationEditPageAsync(id);
            if (model == null)
                return NotFound();

            return PartialView("_CompanyTranslateUpdateViewPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyTranslation(CompanyTranslationEditPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CompanyTranslationUpdateViewPartial", model); 
            }

            var isUpdated = await _companyService.UpdateCompanyTranslation(model);

            if (!isUpdated)
                return NotFound();

            return RedirectToAction(nameof(EditCompanyProfile));
        }

        public async Task<IActionResult> DeleteWorkingField(int id)
        {
            var workingField = await _workingFieldService.GetByIdAsync(id);

            if (workingField == null)
                return BadRequest();

            var deleted = await _workingFieldService.DeleteAsync(id);
            if (deleted)
                return NoContent();
            else return RedirectToAction("CompanyDashboard", "Company");
        }

        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _addressService.GetByIdAsync(id);

            if (address == null)
                return BadRequest();

            var deleted = await _addressService.DeleteAsync(id);
            if (deleted)
                return NoContent();
            else return RedirectToAction("CompanyDashboard", "Company");
        }

        public async Task<IActionResult> AddWorkingField()
        {
            var model = await _companyService.GetWorkingFieldCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkingField(WorkingFieldCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetWorkingFieldCreateViewModel(); 
                return View(model);
            }

            var isCreated = await _companyService.CreateWorkingField(model);
            if (!isCreated) return NotFound();

            return RedirectToAction("CompanyDashboard", "Company");
        }

        public async Task<IActionResult> AddAddress()
        {
            var model = await _companyService.GetAddressCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetAddressCreateViewModel();
                return View(model);
            }

            var isCreated = await _companyService.CreateAddress(model);
            if(!isCreated)
                return NotFound();
            return RedirectToAction("CompanyDashboard", "Company");

        }
    }
}
