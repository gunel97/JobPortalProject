using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
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

        public UserIndexManager(ILanguageService languageService, ICookieService cookieService, IUserService userService)
        {
            _languageService = languageService;
            _cookieService = cookieService;
            _userService = userService;
        }

        public async Task<UserPagedIndexViewModel> GetPagedUserIndexModel(UserFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new UserFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedUsers = await _userService.GetUsers(filter);

            var model = new UserPagedIndexViewModel
            {
                Users = pagedUsers,
                Languages = languages.ToList(),
                Filter = filter,
            };

            return model;
        }
    }
}
