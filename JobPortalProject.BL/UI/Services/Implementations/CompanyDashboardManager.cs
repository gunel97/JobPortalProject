using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class CompanyDashboardManager : ICompanyDashboardService
    {
        private readonly ILanguageService _languageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyService _companyService;
        private readonly ICookieService _cookieService;

        public CompanyDashboardManager(ILanguageService languageService, IHttpContextAccessor httpContextAccessor, ICompanyService companyService, ICookieService cookieService)
        {
            _languageService = languageService;
            _httpContextAccessor = httpContextAccessor;
            _companyService = companyService;
            _cookieService = cookieService;
        }

        public async Task<CompanyDashboardViewModel> GetCompanyDashboardViewModelAsync()
        {
            var selectedLanguage = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync(predicate: x => !x.IsDeleted);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await _companyService.GetAsync(predicate: x => !x.IsDeleted && x.AppUserId == userId,
                include: x => x.Include(c => c.CompanyTranslations.Where(ct => ct.LanguageId == selectedLanguage.Id)));

            var companyDashboardViewModel = new CompanyDashboardViewModel
            {
                Languages = languages.ToList(),
                Name = company.Name,
                LogoUrl=company.LogoUrl,
                IsAccountActive=company.IsAccountActive,
            };

            return companyDashboardViewModel;
        }
    }
}
