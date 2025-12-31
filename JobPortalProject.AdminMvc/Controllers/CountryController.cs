using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryIndexService _countryIndexService;
        private readonly ICountryService _countryService;

        public CountryController(ICountryIndexService countryIndexService, ICountryService countryService)
        {
            _countryIndexService = countryIndexService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(CountryFilterViewModel filter)
        {
            var model = await _countryIndexService.GetPagedCountryIndexModel(filter);
            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            int countryId = int.Parse(id.Split('-').Last());
            var model = await _countryService.GetDetailsViewModel(countryId);
            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _countryService.GetUpdateViewModel(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CountryUpdateViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var result = await _countryService.UpdateAsync(model.Id, model);
            if (!result)
                return View(model);
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _countryService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var result = await _countryService.CreateAsync(model);
            if (result != null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}
