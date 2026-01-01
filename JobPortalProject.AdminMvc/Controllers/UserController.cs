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
        private readonly IUserService _userService;

        public UserController(IUserIndexService userIndexService, ILanguageService languageService, IUserService userService)
        {
            _userIndexService = userIndexService;
            _languageService = languageService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(UserFilterViewModel filter)
        {
            var model = await _userIndexService.GetPagedUserIndexModel(filter);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterViewModel model)
        {
            var indexModel = await _userIndexService.GetPagedUserIndexModel(new UserFilterViewModel());
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index), indexModel);
            var result = await _userService.Register(model);
            if(result.Succeeded) 
            return RedirectToAction(nameof(Index), indexModel);

            return RedirectToAction(nameof(Index), indexModel);
        }
    }
}
