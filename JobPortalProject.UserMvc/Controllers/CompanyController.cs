using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
        private readonly ICompanySocialService _companySocialService;

        public CompanyController(ICompanyDetailsService companyDetailsService, ICompanyListingService companyListingService,
            ICompanyService companyService, ICompanyDashboardService companyDashboardService, ICookieService cookieService,
            IWorkingFieldService workingFieldService, IWorkingFieldTranslationService workingFieldTranslationService,
            IAddressService addressService, ICompanySocialService companySocialService)
        {
            _companyDetailsService = companyDetailsService;
            _companyListingService = companyListingService;
            _companyService = companyService;
            _companyDashboardService = companyDashboardService;
            _cookieService = cookieService;
            _workingFieldService = workingFieldService;
            _workingFieldTranslationService = workingFieldTranslationService;
            _addressService = addressService;
            _companySocialService = companySocialService;
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
        public IActionResult Settings()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> DeleteWorkingField(int id, int languageId)
        {
            var companyId = _companyService.GetCompanyIdOfUser();
            var field = await _workingFieldService.GetByIdAsync(id);
            if (field == null)
                return BadRequest();

            var deleted = await _workingFieldService.DeleteAsync(id);
            if (deleted)
            {
                var model = await _companyService.GetCompanyTranslationEditPageAsync(languageId);
                if (model == null)
                    return NotFound();

                var workingFieldHtml = await RenderPartialViewToString("_WorkingFieldsUpdatePartial", model);

                return Json(new
                {
                    success = true,
                    workingFieldHtml
                });
            }
            else
            {
                return BadRequest();
            }
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

        public async Task<IActionResult> AddTranslation()
        {
            var model = await _companyService.GetAddTranslationToExistedCompanyViewModel(2);
            return PartialView("_AddTranslationModalPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTranslation(AddTranslationToExistedCompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyService.GetAddTranslationToExistedCompanyViewModel(model.LanguageId);
                return PartialView("_AddTranslationModalPartial", model);
            }

            var result = await _companyService.AddTranslationToExistingCompany(model);
            if (result)
                return RedirectToAction(nameof(EditCompanyProfile));
            else
                return PartialView("_AddTranslationModalPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkingField(WorkingFieldCreateViewModel model)
        {
            var companyUpdateModel = await _companyService.GetCompanyUpdateViewModelAsync();
            var companyId = await _companyService.GetCompanyIdOfUser();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction("EditCompanyProfile", companyUpdateModel);
            }

            var created = await _workingFieldService.CreateWorkingField(companyId, model);
            if (!created)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction("Edit company profile", companyUpdateModel);
            }

            TempData["Success"] = "Work Area added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction("EditCompanyProfile", companyUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressCreateViewModel model)
        {
            var companyUpdateModel = await _companyService.GetCompanyUpdateViewModelAsync();
            var companyId = await _companyService.GetCompanyIdOfUser();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction("EditCompanyProfile", companyUpdateModel);
            }

            var created = await _addressService.CreateAddress(companyId, model);
            if (!created)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction("Edit company profile", companyUpdateModel);
            }

            TempData["Success"] = "Address added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction("EditCompanyProfile", companyUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanySocialMedia(CompanySocialCreateViewModel model)
        {
            var companyUpdateModel = await _companyService.GetCompanyUpdateViewModelAsync();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction(nameof(EditCompanyProfile), companyUpdateModel);
            }

            var created = await _companySocialService.CreateAsync(model);
            if (created==null)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction(nameof(EditCompanyProfile), companyUpdateModel);
            }

            TempData["Success"] = "Address added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction("EditCompanyProfile", companyUpdateModel);
        }



        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using var writer = new StringWriter();

            var viewEngine = HttpContext.RequestServices.GetService<ICompositeViewEngine>();
            var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Could not find view '{viewName}'");
            }

            var viewContext = new ViewContext(
                ControllerContext,  // This provides the ViewContext data
                viewResult.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}
