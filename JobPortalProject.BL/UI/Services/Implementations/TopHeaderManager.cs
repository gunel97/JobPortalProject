using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class TopHeaderManager : ITopHeaderService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public TopHeaderManager(ILanguageService languageService, ICookieService cookieService)
        {
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<TopHeaderViewModel> GetTopHeaderViewModelAsync()
        {
            var languages = await _languageService.GetAllAsync(predicate:x=>!x.IsDeleted);
            var selectedLanguage = await _cookieService.GetLanguageAsync();

            var topHeaderViewModel = new TopHeaderViewModel
            {
                Languages = languages.ToList(),
                SelectedLanguage = selectedLanguage
            };

            return topHeaderViewModel;
        }
    }
}
