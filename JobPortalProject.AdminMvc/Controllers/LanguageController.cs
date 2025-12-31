using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)                                             
        {
            _languageService = languageService;
        }

        public async Task<IActionResult> Index()
        {
            var languages = await _languageService.GetAllAsync();

            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LanguageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var result = await _languageService.CreateAsync(model);

            if (result == null)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _languageService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update (int id)
        {
            var model = await _languageService.GetUpdateViewModel(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LanguageUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model= await _languageService.GetUpdateViewModel(model.Id);
                return View(model);
            }

            var result = await _languageService.UpdateAsync(model.Id, model);
            if (!result)
            {
                model = await _languageService.GetUpdateViewModel(model.Id);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
