using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICookieService _cookieService;

        public AccountController(ICookieService cookieService, IUserService userService)
        {
            _cookieService = cookieService;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.LoginAsync(model);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", $"You are banned");

                return View(model);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect.");

                return View(model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            if (result.Succeeded)
            {
                var role = await _userService.GetUserRoleAsync(model.Username);

                if (role == "SuperAdmin" || role=="Admin" || role=="Editor")
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect.");

                    return View(model);
                }
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogOutAsync();

            return RedirectToAction("Login", "Account");
        }

    }
}
