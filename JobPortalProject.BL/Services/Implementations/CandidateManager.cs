using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
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
        private readonly IEnumService _enumService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEducationService _educationService;
        private readonly IEducationTranslationService _educationTranslationService;
        private readonly IExperienceService _experienceService;
        private readonly IExperienceTranslationService _experienceTranslationService;
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public CandidateManager(IRepositoryAsync<Candidate> repository, IMapper mapper,  IEnumService enumService, IHttpContextAccessor httpContextAccessor,    IEducationService educationService, IEducationTranslationService educationTranslationService, IExperienceService experienceService, IExperienceTranslationService experienceTranslationService, ILanguageService languageService, ICookieService cookieService) : base(repository, mapper)
        {
            _enumService = enumService;
            _httpContextAccessor = httpContextAccessor;
            _educationService = educationService;
            _educationTranslationService = educationTranslationService;
            _experienceService = experienceService;
            _experienceTranslationService = experienceTranslationService;
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<Candidate> GetCandidate()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations)
                .Include(x=>x.Resume).ThenInclude(x=>x.PersonalInfo).ThenInclude(x=>x.Address).ThenInclude(x=>x.AddressTranslations)
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
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations.Where(t=>t.LanguageId==languageId))
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations)
                .Include(x => x.Resume).ThenInclude(x => x.Translations.Where(t=>t.LanguageId==languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Experiences).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId)));

            return candidate!;
        }        

        public async Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;

            var educations = candidate.Resume.Educations;
            var models = new List<EducationCreateViewModel>();

            foreach (var education in educations)
            {
                var model = new EducationCreateViewModel
                {
                    IdForTranslation = education.Id,
                    LanguageId = languageId,
                    StartDate = education.StartDate,
                    EndDate = education.EndDate,
                    EducationTypeId = (int)education.EducationType
                };

                models.Add(model);
            }

            var dashboardModel = await GetDashboardViewModel();
            var pageModel = new EducationPageCreateViewModel
            {
                Models = models,
                LanguageId = languageId,
                DashboardModel = dashboardModel,
            };

            return pageModel;
        }

        public async Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId)
        {
            var educationTypes = _enumService.GetEducationTypeListItems();
            var dashboardModel = await GetDashboardViewModel();

            var model = new ExperiencePageCreateViewModel
            {
                LanguageId = languageId,
                DashboardViewModel = dashboardModel,
            };

            return model;
        }

        public async Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;

            var experiences = candidate.Resume.Experiences;
            var models = new List<ExperienceCreateViewModel>();
            var dashboardModel = await GetDashboardViewModel();

            foreach(var experience in experiences)
            {
                var model = new ExperienceCreateViewModel
                {
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate,
                    Translation = new ExperienceTranslationCreateViewModel
                    {
                        ExperienceId = experience.Id,
                    }
                };
                models.Add(model);
            }

            var experiencePageModel = new ExperiencePageCreateViewModel
            {
                Models = models,
                DashboardViewModel = dashboardModel,
                LanguageId = languageId
            };

            return experiencePageModel;
        }

        public async Task<bool> CreateExperienceTranslation(int languageId, ExperiencePageCreateViewModel model)
        {
            var candidate = await GetCandidate();
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            foreach (var experience in model.Models)
            {
                var experienceTranslationCreateModel = new ExperienceTranslationCreateViewModel
                {
                    ExperienceId = experience.Translation.ExperienceId,
                    LanguageId = languageId,
                    CompanyName = experience.Translation.CompanyName,
                    Position = experience.Translation.Position,
                    Responsibility = experience.Translation.Responsibility
                };

                await _experienceTranslationService.CreateAsync(experienceTranslationCreateModel);
            }

            return true;

        }

        public async Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await GetCandidate();
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            foreach(var education in model.Models)
            {
                var educationTranslationCreateModel = new EducationTranslationCreateViewModel
                {
                    EducationId = education.IdForTranslation,
                    LanguageId = languageId,
                    MajorName = education.MajorName,
                    SchoolName = education.SchoolName,
                };

                await _educationTranslationService.CreateAsync(educationTranslationCreateModel);
            }

            return true;
        }

        public async Task<EducationPageCreateViewModel> GetEducationPageCreateViewModel(int languageId)
        {
            var educationTypes = _enumService.GetEducationTypeListItems();
            var dashboardModel = await GetDashboardViewModel();

            var model = new EducationPageCreateViewModel
            {
                EducationTypes = educationTypes,
                LanguageId = languageId,
                DashboardModel=dashboardModel
            };

            return model;
        }

        public async Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await GetCandidate();
            if(candidate ==null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach(var educationCreateViewModel in model.Models)
            {
                var result = await _educationService.Create(candidate.Resume.Id, educationCreateViewModel);

                if (result != null)
                {
                    resultIds.Add(result.Id);
                    var educationTranslationModel = new EducationTranslationCreateViewModel
                    {
                        EducationId = result.Id,
                        LanguageId = languageId,
                        SchoolName = educationCreateViewModel.SchoolName,
                        MajorName = educationCreateViewModel.MajorName,
                    };
                    await _educationTranslationService.CreateAsync(educationTranslationModel);
                }
                else
                {
                    foreach (var id in resultIds)
                    {
                        await _educationService.DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model)
        {
            var candidate = await GetCandidate();

            if(candidate ==null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach (var experienceCreateViewModel in model.Models)
            {
                experienceCreateViewModel.ResumeId = candidate.Resume.Id;
                experienceCreateViewModel.Translation.LanguageId=languageId;
                var result = await _experienceService.CreateAsync(experienceCreateViewModel);
                if (result != null)
                {
                    resultIds.Add(result.Id);
                    experienceCreateViewModel.Translation.ExperienceId = result.Id;
                    await _experienceTranslationService.CreateAsync(experienceCreateViewModel.Translation);
                }
                else
                {
                    foreach (var id in resultIds)
                    {
                        await _experienceService.DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }

        public async Task<CandidateDashboardViewModel> GetDashboardViewModel()
        {
            var languageSelected = await _cookieService.GetLanguageAsync();
            var candidate = await GetCandidate();
            var languages = await _languageService.GetAllAsync();

            var empty = new List<LanguageViewModel>();
            var ready = new List<LanguageViewModel>();

            if (candidate.Resume == null)
            {
                foreach (var language in languages)
                {
                    empty.Add(language);
                }
            }

            else
            {
                foreach (var translation in candidate.Resume.Translations)
                {
                    ready.Add(languages.FirstOrDefault(x => x.Id == translation.LanguageId)!);
                }
                foreach (var language in languages)
                {
                    if (!ready.Contains(language))
                    {
                        empty.Add(language);
                    }
                }
            }

            var model = new CandidateDashboardViewModel
            {
                EmptyLanguages = empty,
                ReadyLanguages = ready
            };

            return model;
        }


    }
}
