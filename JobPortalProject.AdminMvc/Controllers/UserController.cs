using CloudinaryDotNet.Actions;
using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;

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

        public async Task<IActionResult> Index(UserFilterViewModel filter, int page, string role)
        {
            if (page != 0)
                filter.Index = page;
            if(string.IsNullOrEmpty(role))
            filter.Role = role;
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
            if (result.Succeeded)
                return RedirectToAction(nameof(Index), indexModel);

            return RedirectToAction(nameof(Index), indexModel);
        }

        public async Task<IActionResult> Update(string userId)
        {
            var model = await _userService.GetUserUpdateViewModel(userId);
            if (model == null)
                return NotFound();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var adminUser = await _userService.GetCurrentUserAsync();
            if (adminUser == null) return RedirectToAction("Login", "Account");

            var isAdminPassword = await _userService.CheckPasswordAsync(adminUser, model.AdminPassword);
            if (!isAdminPassword)
            {
                ModelState.AddModelError(model.AdminPassword, "Your admin password is incorrect.");
                return View(model);
            }

            var existUserWithEmail = await _userService.GetUserByEmailAsync(model.Email);
            if (existUserWithEmail != null && existUserWithEmail.Id != model.Id)
            {
                ModelState.AddModelError(model.Email, "The new email address is already taken.");
                return View(model);
            }

            var result = await _userService.UpdateUser(model);
            if (result)
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userService.DeleteUserAsync(user);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(nameof(Index));
            }
        }

        public async Task<IActionResult> Deactivate(string id, int page)
        {
            if (id == null)
                return NotFound();
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeactivateUser(user);

            var filter = new UserFilterViewModel();
            filter.Index = page;
            return RedirectToAction(nameof(Index), filter);
        }

        public async Task<IActionResult> Activate(string id, int page)
        {
            if (id == null)
                return NotFound();
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.ActivateUser(user);

            var filter = new UserFilterViewModel();
            filter.Index = page;
            return RedirectToAction(nameof(Index), filter);
        }

        public async Task<IActionResult> Details(string id, int page, string role)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || user.UserName == null)
                return NotFound();

            ViewBag.Index = page;
            ViewBag.Role = role;
         

            var model = new UserViewModel
            {
                Id=user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Role = role,
                IsDeleted = user.IsDeleted,
                CreatedAt=user.CreatedAt
            };

            return View(model);
        }
    }
}
