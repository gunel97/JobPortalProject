using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.BL.ViewModels.UserViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface IUserService
    {
        public Task ActivateUser(AppUser user);
        public Task<IdentityResult> DeleteUserAsync(AppUser user);
        public Task<AppUser> GetUserByIdAsync(string id);
        public Task DeactivateUser(AppUser user);
        public Task<bool> CheckPasswordAsync(AppUser user, string password);
        public Task<IdentityResult> ChangeEmailAsync(AppUser user, ChangeEmailViewModel model);
        public Task<AppUser> GetUserByEmailAsync(string email);
        public Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);
        public Task<string> GetResetPasswordToken(string email);
        public Task<bool> CheckUserByEmail(string email);
        public Task<IdentityResult> Register(UserRegisterViewModel model);
        public Task<PagedResultModel<UserViewModel>> GetPagedUsers(UserFilterViewModel filter);
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
