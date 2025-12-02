using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICookieService _cookieService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICompanyTranslationService _companyTranslationService;
        private readonly ICompanyService _companyService;

        public UserManager(UserManager<AppUser> userManager, ICompanyTranslationService companyTranslationService, ICookieService cookieService, SignInManager<AppUser> signInManager, ICompanyService companyService)
        {
            _userManager = userManager;
            _companyTranslationService = companyTranslationService;
            _cookieService = cookieService;
            _signInManager = signInManager;
            _companyService = companyService;
        }

        public Task<AppUser> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user==null)
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
                    CompanyTypeId = model.CompanyTypeId,
                    AppUserId=user.Id
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
                    

                   var companyTranslation =  await _companyTranslationService.CreateAsync(companyTranslationmodel);

                    if (companyTranslation==null)
                    {
                        await _userManager.DeleteAsync(user);
                        await _companyService.DeleteAsync(company.Id);
                    }
                }
            }

            return result;
        }
    }
}
