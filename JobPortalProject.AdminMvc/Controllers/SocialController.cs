using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.MainSocialViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class SocialController : Controller
    {
        private readonly IMainSocialService _mainSocialService;
        private readonly ILanguageService _languageService;

        public SocialController(IMainSocialService mainSocialService, ILanguageService languageService)
        {
            _mainSocialService = mainSocialService;
            _languageService = languageService;
        }

        public async Task<IActionResult> Index()
        {
            var socials = await _mainSocialService.GetAllAsync();
            var languages = await _languageService.GetAllAsync();

            var model = new SocialIndexViewModel
            {
                Socials = socials.ToList(),
                Languages=languages.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MainSocialCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var result = await _mainSocialService.CreateAsync(model);

            if (result == null)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _mainSocialService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _mainSocialService.GetUpdateViewModel(id);
            if (model == null)
                return NotFound();
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(MainSocialUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mainSocialService.UpdateAsync(model.Id, model);
            if (!result)
                return View(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
