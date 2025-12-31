using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityIndexService _cityIndexService;
        private readonly ICityService _cityService;

        public CityController(ICityIndexService cityIndexService, ICityService cityService)
        {
            _cityIndexService = cityIndexService;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index(CityFilterViewModel filter)
        {
            var model = await _cityIndexService.GetPagedCityIndexModel(filter);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var result = await _cityService.CreateAsync(model);
            if (result == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            int cityId = int.Parse(id.Split('-').Last());
            var model = await _cityService.GetDetailsViewModel(cityId);
            if(model==null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _cityService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _cityService.GetUpdateViewModel(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CityUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _cityService.UpdateAsync(model.Id, model);
            if (!result)
                return View(model);
            else
                return RedirectToAction(nameof(Index));
        }
    }
}

