using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CandidateManager : CrudManager<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
        , ICandidateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public CandidateManager(IRepositoryAsync<Candidate> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILanguageService languageService, ICookieService cookieService) : base(repository, mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<Candidate> GetCandidate()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x
                .Include(x=>x.AppUser)
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations)
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations)
                .Include(x => x.Resume).ThenInclude(x => x.Translations)
                .Include(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations)
                .Include(x => x.Resume).ThenInclude(x => x.Experiences).ThenInclude(x => x.Translations));

            return candidate!;
        }

        public async Task<Candidate> GetCandidateWithTranslation(int languageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations)
                .Include(x => x.Resume).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Experiences).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId)));

            return candidate!;
        }

        public async Task<CandidateDashboardViewModel> GetDashboardViewModel()
        {
            var languageSelected = await _cookieService.GetLanguageAsync();
            var candidate = await GetCandidate();
            var languages = await _languageService.GetAllAsync();
            var imageUrl = "";

            var empty = new List<LanguageViewModel>();
            var ready = new List<LanguageViewModel>();
            var resumeId = 0;
            if (candidate.Resume == null)
            {
                foreach (var language in languages)
                {
                    empty.Add(language);
                }
            }

            else
            {
                resumeId = candidate.Resume.Id;
                foreach (var translation in candidate.Resume.Translations)
                {
                    if (translation.IsCompleted)
                        ready.Add(languages.FirstOrDefault(x => x.Id == translation.LanguageId)!);
                }
                foreach (var language in languages)
                {
                    if (!ready.Any(x => x.Id == language.Id))
                    {
                        empty.Add(language);
                    }
                }
                if (candidate.Resume.PersonalInfo != null)
                    imageUrl = candidate.Resume.PersonalInfo.ImageUrl;
            }

            var model = new CandidateDashboardViewModel
            {
                CandidateId= candidate.Id,
                ImageUrl=imageUrl,
                UserName = candidate.AppUser!.UserName,
                ResumeId = resumeId,
                EmptyLanguages = empty,
                ReadyLanguages = ready
            };

            return model;
        }

    }
}
