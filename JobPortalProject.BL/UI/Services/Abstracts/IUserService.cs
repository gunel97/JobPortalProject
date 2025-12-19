using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface IUserService
    {
        public Task<IdentityResult> RegisterCompanyAsync(CompanyRegisterViewModel model);
        public Task<IdentityResult> RegisterCandidateAsync(UserRegisterViewModel model);
        public Task<CompanyRegisterViewModel> GetCompanyRegisterViewModel();
        public Task<string> GetUserRoleAsync(string username);
        public Task<SignInResult> LoginAsync(LoginViewModel model);
        public Task LogOutAsync();
        public Task<AppUser> GetCurrentUserAsync();
        public Task<IdentityResult> ChangePasswordAsync(AppUser user, ChangePasswordViewModel model);
    }
}
