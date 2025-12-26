using AutoMapper;
using AutoMapper.Internal;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ExperienceManager:CrudManager<Experience, ExperienceViewModel, ExperienceCreateViewModel, ExperienceUpdateViewModel>
        , IExperienceService
    {
        private readonly IExperienceTranslationService _experienceTranslationService;
        private readonly ICandidateService _candidateService;
        private readonly ICookieService _cookieService;

        public ExperienceManager(IRepositoryAsync<Experience> repository, IMapper mapper, IExperienceTranslationService experienceTranslationService, ICandidateService candidateService, ICookieService cookieService) : base(repository, mapper)
        {
            _experienceTranslationService = experienceTranslationService;
            _candidateService = candidateService;
            _cookieService = cookieService;
        }

        public async Task<List<ExperienceViewModel>> GetExperienceModelsOfResume(int resumeId)
        {
            var models = new List<ExperienceViewModel>();
            var experiences = await GetAllAsync(predicate: x => x.ResumeId == resumeId);
            foreach (var experience in experiences)
            {
                var model = await GetExperienceViewModel(experience.Id);
                models.Add(model);
            }

            return models;
        }

        public async Task<ExperienceViewModel> GetExperienceViewModel(int experienceId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var experience = await Repository.GetAsync(predicate: x => x.Id == experienceId,
                include: x => x.Include(x => x.Translations.Where(t => t.LanguageId == language.Id)));

            if (experience == null || !experience.Translations.Any())
                return null!;

            var model = new ExperienceViewModel
            {
                Id = experience.Id,
                ResumeId = experience.ResumeId,
                StartDate = experience.StartDate,
                EndDate = experience.EndDate,
                CompanyName = experience.Translations.FirstOrDefault()!.CompanyName,
                Position = experience.Translations.FirstOrDefault()!.Position,
                Responsibility = experience.Translations.FirstOrDefault()!.Responsibility
            };

            return model;
        }

        public async Task<bool> AddExperienceToResume(ExperienceAddViewModel model, int resumeId)
        {
            var experience = new Experience
            {
                ResumeId = resumeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
            };
            var result = await Repository.AddAsync(experience);

            if (result == null)
                return false;
            foreach(var translation in model.Translations)
            {
                translation.ExperienceId = result.Id;
                var translationResult = await _experienceTranslationService.CreateAsync(translation);
            }

            return true;
        }

        public async Task<ExperienceUpdatePageViewModel> GetExperienceUpdateViewModel()
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;
            var dashboard = await _candidateService.GetDashboardViewModel();
            var updateModels = new List<ExperienceUpdateViewModel>();
            var model = new ExperienceUpdatePageViewModel();
            model.Dashboard = dashboard;

            if (!candidate.Resume.Experiences.Any() || candidate.Resume.Experiences == null)
            {
                model.Models = updateModels;
                return model;
            }

            foreach (var experience in candidate.Resume.Experiences)
            {
                var updateModel = new ExperienceUpdateViewModel
                {
                    Id = experience.Id,
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate,
                    Translations = experience.Translations.Select(x => new ExperienceTranslationUpdateViewModel
                    {
                        ExperienceId = experience.Id,
                        LanguageId = x.LanguageId,
                        Responsibility = x.Responsibility,
                        CompanyName = x.CompanyName,
                        Position = x.Position,
                        LangIcon = dashboard.ReadyLanguages.FirstOrDefault(t => t.Id == x.LanguageId).IconUrl
                    }).ToList()
                };

                updateModels.Add(updateModel);
            }

            model.Models = updateModels;
            return model;

        }

        public async Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId)
        {
            var dashboardModel = await _candidateService.GetDashboardViewModel();

            var model = new ExperiencePageCreateViewModel
            {
                LanguageId = languageId,
                DashboardViewModel = dashboardModel,
            };

            return model;
        }

        public async Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;

            var experiences = candidate.Resume.Experiences;
            var models = new List<ExperienceCreateViewModel>();
            var dashboardModel = await _candidateService.GetDashboardViewModel();

            foreach (var experience in experiences)
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
            var candidate = await _candidateService.GetCandidate();
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

        public async Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();

            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach (var experienceCreateViewModel in model.Models)
            {
                experienceCreateViewModel.ResumeId = candidate.Resume.Id;
                experienceCreateViewModel.Translation.LanguageId = languageId;
                var result = await CreateAsync(experienceCreateViewModel);
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
                        await DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
