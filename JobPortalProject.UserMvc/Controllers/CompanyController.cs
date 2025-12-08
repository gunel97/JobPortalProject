using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
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

        public CompanyController(ICompanyDetailsService companyDetailsService, ICompanyListingService companyListingService, ICompanyService companyService, ICompanyDashboardService companyDashboardService, ICookieService cookieService, IWorkingFieldService workingFieldService, IWorkingFieldTranslationService workingFieldTranslationService)
        {
            _companyDetailsService = companyDetailsService;
            _companyListingService = companyListingService;
            _companyService = companyService;
            _companyDashboardService = companyDashboardService;
            _cookieService = cookieService;
            _workingFieldService = workingFieldService;
            _workingFieldTranslationService = workingFieldTranslationService;
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

        public async Task<IActionResult> EditCompanyProfile(int selectedLanguageId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var model = await _companyService.GetCompanyUpdateViewModelAsync(selectedLanguageId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(int id, CompanyUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetCompanyUpdateViewModelAsync(model.SelectedUpdateLanguageId);

                return View(model);
            }

            var isUpdated = await _companyService.UpdateAsync(model.SelectedUpdateLanguageId, model);
          
            if (!isUpdated)
                return NotFound();

            return RedirectToAction("CompanyDashboard", "Company");
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

        public async Task<IActionResult> AddWorkingField(int id)
        {
            var model = await _companyService.GetWorkingFieldCreateViewModel(id);

            return PartialView("_WorkingFieldAddPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkingField(int id, WorkingFieldCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _companyService.GetWorkingFieldCreateViewModel(model.SelectedUpdateLanguageId);

                return PartialView("_WorkingFieldAddPartial", model);
            }

            var isCreated = await _companyService.CreateWorkingField(model);
            if (!isCreated) return NotFound();

            return NoContent();
        }

        public async Task<IActionResult> AddWorkingFieldTranslation(int id)
        {
            var model = await _companyService.GetAddTranslationViewModelAsync(id);

            return PartialView("_AddWorkingFieldTranslationPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkingFieldTranslation(AddWorkingFieldTranslationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _companyService.GetAddTranslationViewModelAsync(model.SelectedLanguageId);

                return PartialView("_AddWorkingFieldTranslationPartial", model);
            }

            if (model.TranslationCreateViewModel != null)
            {
                var translation = await _workingFieldService.CreateWorkingFieldTranslationAsync(model);

                if (translation == null)
                    return NotFound();
            }
            else
            {
                await _companyService.GetAddTranslationViewModelAsync(model.SelectedLanguageId);

                return PartialView("_AddWorkingFieldTranslationPartial", model);
            }

            return NoContent();
        }
    }
}
