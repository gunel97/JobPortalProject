using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.Services.Implementations;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Mailing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.UserMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly ICookieService _cookieService;
        private readonly IMailService _mailService;

        public AccountController(IUserService userService, ICompanyService companyService, ICookieService cookieService, IMailService mailService)
        {
            _userService = userService;
            _companyService = companyService;
            _cookieService = cookieService;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RegisterCompany()
        {
            var model = await _userService.GetCompanyRegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyRegisterViewModel model)
        {
            var language = await _cookieService.GetLanguageAsync();
            var existedCompany = await _companyService.GetAsync(
                predicate: x =>
                x.CompanyTranslations.FirstOrDefault(a => a.LanguageId == language.Id)!.Name == model.CompanyName);

            if (existedCompany != null)
            {
                ModelState.AddModelError("", "This company name exists");

                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.RegisterCompanyAsync(model);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RegisterCandidate()
        {
            var model = new UserRegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCandidate(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.RegisterCandidateAsync(model);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(Login));
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
                    return RedirectToAction("Dashboard", "Company");
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Candidate, Company")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Candidate, Company")]
        public IActionResult Settings()
        {
            return View();
        }

        [Authorize(Roles ="Candidate, Company")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(AccountSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Settings", model);

            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (model.ChangePasswordModel == null)
                return View("Settings", model);

            var result = await _userService.ChangePasswordAsync(user, model.ChangePasswordModel);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(model);
            }

            await _userService.LogOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles ="Candidate, Company")]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(AccountSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Settings", model);

            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Login", "Account");

            if (model.ChangeEmailModel == null)
                return View("Settings", model);

            var existingUserWithNewEmail = await _userService.GetUserByEmailAsync(model.ChangeEmailModel.NewEmail);
            if (existingUserWithNewEmail != null && existingUserWithNewEmail.Id != user.Id)
            {
                ModelState.AddModelError(model.ChangeEmailModel.NewEmail, "This email address is already taken.");
                return View("Settings", model);
            }

            var checkPasswordResult = await _userService.CheckPasswordAsync(user, model.ChangeEmailModel.CurrentPasswordForEmail);
            if(!checkPasswordResult)
            {
                ModelState.AddModelError("", "Password is not correct");
                return View("Settings", model);
            }

            var result = await _userService.ChangeEmailAsync(user, model.ChangeEmailModel);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View("Settings", model);
            }

            return RedirectToAction("Dashboard", "Company");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ForgotPasswordResult()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is required");

                return View();
            }

            var result = await _userService.CheckUserByEmail(email);
            if(!result)
            {
                ModelState.AddModelError("", "User not found");

                return View();
            }

            var resetToken = await _userService.GetResetPasswordToken(email);
            var resetLink = Url.Action("ResetPassword", "Account", new { email, resetToken },
                Request.Scheme, Request.Host.ToString());

            _mailService.SendMail(new Mail
            {
                ToEmail = email,
                Subject = "Reset Password Job Portal",
                TextBody = resetLink
            });

            return View(nameof(ForgotPasswordResult));
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userResult = await _userService.CheckUserByEmail(model.Email);
            if (!userResult)
                return NotFound();

            var result = await _userService.ResetPassword(model);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }
    }
}
