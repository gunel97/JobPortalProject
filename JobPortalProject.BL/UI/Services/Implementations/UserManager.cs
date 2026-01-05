using CloudinaryDotNet;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.BL.ViewModels.UserViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ResetPasswordAsync(user!, model.ResetToken, model.NewPassword);

            return result;
        }

        public async Task<bool> CheckUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            else
                return true;
        }

        public async Task<string> GetResetPasswordToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user!);
            return resetPasswordToken;
        }

        public async Task<PagedResultModel<UserViewModel>> GetUsers(UserFilterViewModel filter)
        {
            // 1. Build Predicate and OrderBy
            Expression<Func<AppUser, bool>> predicate = BuildPredicate(filter);
            Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy = BuildOrderBy(filter);

            // 2. Prepare the Query
            var query = _userManager.Users.AsQueryable();

            // 3. Apply Filtering
            query = query.Where(predicate);

            // 4. Apply Sorting
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // 5. Get Count for Pagination
            var totalCount = await query.CountAsync();

            // 6. Get the Paged Data
            var users = await query
                .Skip(filter.Index * filter.Size)
                .Take(filter.Size)
                .ToListAsync();

            // 7. Map to ViewModel
            var userModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                // NOTE: Role filtering "!= SuperAdmin" is best done in the database, 
                // but since GetUserRoleAsync is an async helper, we do it here. 
                // Ideally, you should filter this in the SQL query above using .Where() if possible.
                var role = await GetUserRoleAsync(user.UserName);

                if (role != "SuperAdmin")
                {
                    var model = new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = role
                    };
                    userModels.Add(model);
                }
            }

            // 8. Return Result

            var Users = new PagedResultModel<UserViewModel>
            {
                Items = userModels,
                Index = filter.Index,
                Size = filter.Size,
                Count = totalCount,
                // Calculate pages: Ceiling(Total / Size)
                Pages = (int)Math.Ceiling(totalCount / (double)filter.Size)
            };

            return Users;
        }

        private Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> BuildOrderBy(UserFilterViewModel filter)
        {
            var sortBy = filter.SortBy?.ToLower().Trim() ?? "createdat";
            var sortOrder = filter.SortOrder?.ToLower().Trim() ?? "desc";

            return queryable =>
            {
                IOrderedQueryable<AppUser> ordered;

                switch (sortBy)
                {
                    case "name":
                        // Sort by First Name, then Last Name
                        if (sortOrder == "asc")
                        {
                            ordered = queryable.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                        }
                        else
                        {
                            ordered = queryable.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);
                        }
                        break;

                    case "username":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.UserName)
                            : queryable.OrderByDescending(x => x.UserName);
                        break;

                    case "email":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Email)
                            : queryable.OrderByDescending(x => x.Email);
                        break;

                    case "createdat":
                    default:
                        // Assuming you have an Id or CreatedAt to sort by default
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Id)
                            : queryable.OrderByDescending(x => x.Id);
                        break;
                }

                return ordered;
            };
        }

        private Expression<Func<AppUser, bool>> BuildPredicate(UserFilterViewModel filter)
        {
            var term = filter.SearchTerm?.ToLower().Trim();

            Expression<Func<AppUser, bool>> predicate = x =>
                string.IsNullOrEmpty(term) ||
                (x.FirstName != null && x.FirstName.ToLower().Contains(term)) ||
                (x.LastName != null && x.LastName.ToLower().Contains(term)) ||
                (x.Email != null && x.Email.ToLower().Contains(term)) ||
                (x.UserName != null && x.UserName.ToLower().Contains(term));

            return predicate;
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
        
        public async Task<IdentityResult> Register(UserRegisterViewModel model)
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
                await _userManager.AddToRoleAsync(user, model.UserType);
            
            return result;
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
                    MemberSince=DateTime.UtcNow
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

        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
          return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, ChangePasswordViewModel model)
        {
            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<IdentityResult> ChangeEmailAsync(AppUser user, ChangeEmailViewModel model)
        {
            return await _userManager.SetEmailAsync(user, model.NewEmail);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user!;
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
