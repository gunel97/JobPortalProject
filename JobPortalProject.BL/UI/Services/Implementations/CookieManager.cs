using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class CookieManager : ICookieService
    {
        private readonly ILanguageService _languageService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieManager(ILanguageService languageService, IHttpContextAccessor contextAccessor)
        {
            _languageService = languageService;
            _contextAccessor = contextAccessor;
        }

        public async Task<LanguageViewModel> GetLanguageAsync()
        {
            var languages = await _languageService.GetAllAsync();
            var culture = _contextAccessor.HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var isoCode = culture?.Substring(culture.LastIndexOf("=") + 1) ?? "en-Us";
            var selectedLanguage = await _languageService.GetAsync(x => x.IsoCode == isoCode);

            return selectedLanguage;
        }
    }
}
