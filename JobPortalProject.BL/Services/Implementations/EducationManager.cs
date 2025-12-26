using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class EducationManager:CrudManager<Education, EducationViewModel, EducationCreateViewModel, EducationUpdateViewModel>
        , IEducationService
    {
        private readonly IEducationTranslationService _educationTranslationService;
        private readonly ICandidateService _candidateService;
        private readonly IEnumService _enumService;
        private readonly ICookieService _cookieService;

        public EducationManager(IRepositoryAsync<Education> repository, IMapper mapper, IEducationTranslationService educationTranslationService, ICandidateService candidateService, IEnumService enumService, ICookieService cookieService) : base(repository, mapper)
        {
            _educationTranslationService = educationTranslationService;
            _candidateService = candidateService;
            _enumService = enumService;
            _cookieService = cookieService;
        }

        public async Task<List<EducationViewModel>> GetEducationModelsOfResume(int resumeId)
        {
            var models = new List<EducationViewModel>();
            var educations = await GetAllAsync(predicate: x => x.ResumeId == resumeId);
            foreach (var education in educations)
            {
                var model = await GetEducationViewModel(education.Id);
                models.Add(model);
            }

            return models;
        }

        public async Task<EducationViewModel> GetEducationViewModel(int educationId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var education = await Repository.GetAsync(predicate: x => x.Id==educationId,
                include: x => x.Include(x => x.Translations.Where(t => t.LanguageId == language.Id)));

            if (education == null || !education.Translations.Any())
                return null!;

            var model = new EducationViewModel
            {
                Id = education.Id,
                ResumeId = education.ResumeId,
                SchoolName = education.Translations.FirstOrDefault().SchoolName,
                MajorName = education.Translations.FirstOrDefault().MajorName,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                EducationType=education.EducationType.ToString()
            };

            return model;
        }

        public async Task<Education> Create(int resumeId, EducationCreateViewModel model)
        {
            var createdsList = new List<Education>();

            var education = new Education
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ResumeId = resumeId,
                EducationType = (EducationType)model.EducationTypeId
            };

            var result = await Repository.AddAsync(education);
            return result;
        }

        public async Task<bool> AddEducationToResume(EducationAddViewModel model, int resumeId)
        {
            var education = new Education
            {
                ResumeId = resumeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                EducationType = (EducationType)model.EducationTypeId
            };

            var result = await Repository.AddAsync(education);

            if (result == null)
                return false;

            foreach (var translation in model.Translations)
            {
                translation.EducationId = result.Id;
                var translationResult = await _educationTranslationService.CreateAsync(translation);
            }

            return true;
        }

        public async Task<EducationUpdatePageViewModel> GetEducationUpdateViewModel()
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;
            var dashboard = await _candidateService.GetDashboardViewModel();
            var educationTypes = _enumService.GetEducationTypeListItems();
            var updateModels = new List<EducationUpdateViewModel>();
            var model = new EducationUpdatePageViewModel();
            model.DashboardModel = dashboard;
            model.EducationTypes = educationTypes;

            if (!candidate.Resume.Educations.Any() || candidate.Resume.Educations == null)
            {
                model.UpdateModels = updateModels;
                return model;
            }

            foreach (var education in candidate.Resume.Educations)
            {
                updateModels.Add(new EducationUpdateViewModel
                {
                    Id = education.Id,
                    EducationTypeId = (int)education.EducationType,
                    StartDate = education.StartDate,
                    EndDate = education.EndDate,
                    Translations = education.Translations.Select(x => new EducationTranslationUpdateViewModel
                    {
                        Id = x.Id,
                        LangIcon = dashboard.ReadyLanguages.FirstOrDefault(t => t.Id == x.LanguageId)!.IconUrl,
                        EducationId = education.Id,
                        LanguageId = x.LanguageId,
                        MajorName = x.MajorName,
                        SchoolName = x.SchoolName,
                    }).ToList()
                });
            }

            model.UpdateModels = updateModels;
            return model;
        }

        public async Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await _candidateService.GetCandidate();
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

            var dashboardModel = await _candidateService.GetDashboardViewModel();
            var pageModel = new EducationPageCreateViewModel
            {
                Models = models,
                LanguageId = languageId,
                DashboardModel = dashboardModel,
            };

            return pageModel;
        }

        public async Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            foreach (var education in model.Models)
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
            var dashboardModel = await _candidateService.GetDashboardViewModel();

            var model = new EducationPageCreateViewModel
            {
                EducationTypes = educationTypes,
                LanguageId = languageId,
                DashboardModel = dashboardModel
            };

            return model;
        }

        public async Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach (var educationCreateViewModel in model.Models)
            {
                var result = await Create(candidate.Resume.Id, educationCreateViewModel);

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
                        await DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }


    }
}
