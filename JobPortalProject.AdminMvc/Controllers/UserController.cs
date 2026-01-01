using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserIndexService _userIndexService;
        private readonly ILanguageService _languageService;

        public UserController(IUserIndexService userIndexService, ILanguageService languageService)
        {
            _userIndexService = userIndexService;
            _languageService = languageService;
        }

        public async Task<IActionResult> Index(UserFilterViewModel filter)
        {
            var model = await _userIndexService.GetPagedUserIndexModel(filter);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }
    }
}
