using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class UserIndexManager : IUserIndexService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserIndexManager(ILanguageService languageService, ICookieService cookieService, IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _languageService = languageService;
            _cookieService = cookieService;
            _userService = userService;
            _roleManager = roleManager;
        }

        public async Task<UserPagedIndexViewModel> GetPagedUserIndexModel(UserFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            filter ??= new UserFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedUsers = await _userService.GetPagedUsers(filter);

            var model = new UserPagedIndexViewModel
            {
                Users = pagedUsers,
                Languages = languages.ToList(),
                Filter = filter,
                Roles=roles
            };

            return model;
        }

      
    }
}
