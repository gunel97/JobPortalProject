using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class SidebarLanguageManager:ISidebarLanguageService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public SidebarLanguageManager(ICookieService cookieService, ILanguageService languageService)
        {
            _cookieService = cookieService;
            _languageService = languageService;
        }

        public async Task<TopHeaderViewModel> GetSidebarLanguageModelAsync()
        {
            var languages = await _languageService.GetAllAsync(predicate: x => !x.IsDeleted);
            var selectedLanguage = await _cookieService.GetLanguageAsync();

            var topHeaderViewModel = new TopHeaderViewModel
            {
                Languages = languages.ToList(),
                SelectedLanguage = selectedLanguage,
            };

            return topHeaderViewModel;
        }
    }

}
