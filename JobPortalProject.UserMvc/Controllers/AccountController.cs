using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.Services.Implementations;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.UserMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyDashboardService _companyDashboardService;
        private readonly IUserService _userService;
        private readonly ICookieService _cookieService;

        public AccountController(IUserService userService, ICompanyService companyService, ICookieService cookieService, ICompanyDashboardService companyDashboardService)
        {
            _userService = userService;
            _companyService = companyService;
            _cookieService = cookieService;
            _companyDashboardService = companyDashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyRegisterViewModel model)
        {
            var language = await _cookieService.GetLanguageAsync();
            var existedCompany = await _companyService.GetAsync(
                predicate: x =>
                x.CompanyTranslations.FirstOrDefault(a=>a.LanguageId==language.Id)!.Name==model.CompanyName);

            if (existedCompany != null)
            {
                ModelState.AddModelError("", "This company name exists");

                return View(model);
            }

            if(!ModelState.IsValid) 
                return View(model);

            var result = await _userService.RegisterCompanyAsync(model);

            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }    

                return View(model);
            }

            return RedirectToAction("CompanyDashboard", "Account");
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

                if (role == "Company")
                    return RedirectToAction("CompanyDashboard", "Account");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CompanyDashboard()
        {
            var model = await _companyDashboardService.GetCompanyDashboardViewModelAsync();

            return View(model);
        }

        public async Task<IActionResult> EditCompanyProfile()
        {
            var model = await _companyService.GetCompanyUpdateViewModelAsync(1);

            return View(model);
        }
    } 
}
