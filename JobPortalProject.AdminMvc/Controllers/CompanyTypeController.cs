using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class CompanyTypeController : Controller
    {
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICompanyTypeIndexService _companyTypeIndexService;

        public CompanyTypeController(ICompanyTypeService companyTypeService, ICompanyTypeIndexService companyTypeIndexService)
        {
            _companyTypeService = companyTypeService;
            _companyTypeIndexService = companyTypeIndexService;
        }

        public async Task<IActionResult> Index(CompanyTypeFilterViewModel filter)
        {
            var model = await _companyTypeIndexService.GetPagedCompanyTypeIndexModel(filter);

            return View(model);
        }

        public async Task<IActionResult> Details (string id)
        {
            int Id = int.Parse(id.Split('-').Last());
            var model = await _companyTypeService.GetDetailsViewModel(Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _companyTypeService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _companyTypeService.GetUpdateViewModel(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CompanyTypeUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _companyTypeService.GetUpdateViewModel(model.Id);
                return View(model);
            }

            var result = await _companyTypeService.UpdateCompanyTypeAsync(model);
            if(result)
                return RedirectToAction(nameof(Index));
            else
            {
                model = await _companyTypeService.GetUpdateViewModel(model.Id);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyTypeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var result = await _companyTypeService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
