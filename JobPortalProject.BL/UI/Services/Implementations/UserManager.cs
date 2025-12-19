using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICookieService _cookieService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyTranslationService _companyTranslationService;
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICandidateService _candidateService;

        public UserManager(UserManager<AppUser> userManager, ICompanyTranslationService companyTranslationService, ICookieService cookieService, SignInManager<AppUser> signInManager, ICompanyService companyService, IHttpContextAccessor httpContextAccessor, ICompanyTypeService companyTypeService, ICandidateService candidateManager)
        {
            _userManager = userManager;
            _companyTranslationService = companyTranslationService;
            _cookieService = cookieService;
            _signInManager = signInManager;
            _companyService = companyService;
            _httpContextAccessor = httpContextAccessor;
            _companyTypeService = companyTypeService;
            _candidateService = candidateManager;
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var role = await _userManager.GetRolesAsync(user!);

            return role.FirstOrDefault()!;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        
        public async Task<IdentityResult> RegisterCompanyAsync(CompanyRegisterViewModel model)
        {
            var language = await _cookieService.GetLanguageAsync();
            
            var user = new AppUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
               await _userManager.AddToRoleAsync(user, "Company");

                var companyModel = new CompanyCreateViewModel
                {
                    AppUserId=user.Id,
                    CompanyTypeId=model.CompanyTypeId,
                };

                var company = await _companyService.CreateAsync(companyModel);

                if (company != null)
                {

                    var companyTranslationmodel = new CompanyTranslationCreateViewModel
                    {
                        CompanyId = company.Id,
                        LanguageId = language.Id,
                        Name = model.CompanyName
                    };


                    var companyTranslation = await _companyTranslationService.CreateAsync(companyTranslationmodel);

                    if (companyTranslation == null)
                    {
                        await _userManager.DeleteAsync(user);
                        await _companyService.DeleteAsync(company.Id);
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "CompanyCreationFailed",
                        Description = "Failed to create company."
                    });
                }
            }

            return result;
        }

        public async Task<IdentityResult> RegisterCandidateAsync(UserRegisterViewModel model)
        {
            var user = new AppUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Candidate");

                var candidateModel = new CandidateCreateViewModel
                {
                    AppUserId = user.Id,
                };

                var candidate = await _candidateService.CreateAsync(candidateModel);

                if (candidate == null)
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "CompanyCreationFailed",
                        Description = "Failed to create company."
                    });
                }

            }

                return result;     
        }

        public async Task<CompanyRegisterViewModel> GetCompanyRegisterViewModel()
        {
            var language = await _cookieService.GetLanguageAsync();
            var companyTypesList = await _companyTypeService.GetCompanyTypeSelectListItems(language.Id);

            var model = new CompanyRegisterViewModel
            {
                CompanyTypesList = companyTypesList
            };

            return model;
        }

        public async Task<CompanyViewModel> GetCompanyIdOfUserAsync(AppUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var company = await _companyService.GetAsync(predicate: x => x.AppUser!.Id == userId);

            return company;
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, ChangePasswordViewModel model)
        {
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return null!;

            return await _userManager.GetUserAsync(user);
        }
    }
}
