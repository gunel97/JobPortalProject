using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.DA.DataContext.Enums;
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
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IJobService _jobService;

        public CompanyDashboardManager(ILanguageService languageService, IHttpContextAccessor httpContextAccessor, ICompanyService companyService, ICookieService cookieService, IJobApplicationService jobApplicationService, IJobService jobService)
        {
            _languageService = languageService;
            _httpContextAccessor = httpContextAccessor;
            _companyService = companyService;
            _cookieService = cookieService;
            _jobApplicationService = jobApplicationService;
            _jobService = jobService;
        }

        public async Task<CompanyDashboardViewModel> GetCompanyDashboardViewModelAsync()
        {
            var selectedLanguage = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync(predicate: x => !x.IsDeleted);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await _companyService.GetAsync(predicate: x => !x.IsDeleted && x.AppUserId == userId,
                include: x => x.Include(c => c.CompanyTranslations.Where(ct => ct.LanguageId == selectedLanguage.Id)));
            var applicants = await _jobApplicationService.GetApplicantsOfCompany(company.Id);
            var jobs = await _jobService.GetActiveJobsOfCompanyAsync(company.Id);
            
            var companyDashboardViewModel = new CompanyDashboardViewModel
            {
                CompanyId=company.Id,
                Languages = languages.ToList(),
                Name = company.Name,
                LogoUrl=company.LogoUrl,
                IsAccountActive=company.IsAccountActive,
                TotalApplicantCount=applicants.Count(),
                TotalAcceptedCount=applicants.Where(x=>x.Status==((JobApplicationStatus)3).ToString()).Count(),
                WaitingInterviewCount=applicants.Where(x=>x.Status==((JobApplicationStatus)2).ToString()).Count(),
                Applicants= applicants.OrderByDescending(x=>x.AppliedAt).Take(5).ToList(),
                ActiveJobCount=jobs.Where(x=>!x.Expired).Count(),
                IsMembershipActive=company.IsMembershipActive,
                MembershipExpiresAt=company.MembershipExpiresAt,
                ReadyLanguages=await _companyService.GetReadyLanguagesOfCompany(company.Id),
                EmptyLanguages=await _companyService.GetEmptyLanguagesOfCompany(company.Id)
            };

            return companyDashboardViewModel;
        }
    }
}
