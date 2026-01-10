using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class CompanyAdminManager : ICompanyAdminService
    {
        private readonly ILanguageService _languageService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly IUserService _userService;
        private readonly ICookieService _cookieService;
        private readonly IJobService _jobService;
        private readonly IJobApplicationService _jobApplicationService;

        public CompanyAdminManager(ILanguageService languageService, ICompanyDetailsService companyDetailsService, ICompanyService companyService, IUserService userService, ICookieService cookieService, IJobService jobService, IJobApplicationService jobApplicationService)
        {
            _languageService = languageService;
            _companyDetailsService = companyDetailsService;
            _companyService = companyService;
            _userService = userService;
            _cookieService = cookieService;
            _jobService = jobService;
            _jobApplicationService = jobApplicationService;
        }


        public async Task<CompanyDetailsAdminViewModel> GetDetailsAdminViewModel(string userId, int languageId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();
            if (user == null)
                return null!;

            var company = await _companyService.GetAsync(predicate: x => x.AppUserId == user.Id);

            if (company == null)
                return null!;

            var model = new CompanyDetailsAdminViewModel();

            if (languageId == 0)
                languageId = language.Id;

            var readyLanguages = await _companyService.GetReadyLanguagesOfCompany(company.Id);
            var lang = await _languageService.GetByIdAsync(languageId);
            if (readyLanguages.Any(x=>x.Id==lang.Id))
            {
                model.Details = await _companyDetailsService.GetCompanyDetailsAsync(company.Id, languageId);
            }
            else
            {
                languageId = readyLanguages.FirstOrDefault()!.Id;
                model.Details=await _companyDetailsService.GetCompanyDetailsAsync(company.Id,languageId);
            }
            if (model.Details == null)
                return null!;

            model.SelectedLanguage = languages.FirstOrDefault(x => x.Id == languageId);
            model.Username = user.UserName;
            model.UserId = userId;

            var jobs = await _jobService.GetAllAsync(predicate: x=>x.CompanyId== company.Id);

            model.TotalJobCount = jobs.ToList().Count();
            model.ExpiredJobCount = jobs.Where(x => x.ExpirationDate < DateTime.UtcNow).Count();

            var jobApplications = await _jobApplicationService.GetApplicantsOfCompany(company.Id);
            model.TotalJobApplications= jobApplications.ToList().Count();
            model.AcceptedJobApplications = jobApplications.Where(x => x.Status.ToString() == "accepted").Count();

            return model;
        }
    }
}
