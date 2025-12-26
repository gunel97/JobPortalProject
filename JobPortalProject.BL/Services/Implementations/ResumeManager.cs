using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ResumeManager:CrudManager<Resume, ResumeViewModel, ResumeCreateViewModel, ResumeUpdateViewModel>
        , IResumeService
    {
        private readonly ICookieService _cookieService;
        private readonly IExperienceService _experienceService;
        private readonly IPersonalInfoService _personalInfoService;
        private readonly IEducationService _educationService;

        public ResumeManager(IRepositoryAsync<Resume> repository, IMapper mapper, ICookieService cookieService, IExperienceService experienceService, IPersonalInfoService personalInfoService, IEducationService educationService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _experienceService = experienceService;
            _personalInfoService = personalInfoService;
            _educationService = educationService;
        }

        public async Task<ResumeViewModel> GetResumeBase(int id)
        {
            var language = await _cookieService.GetLanguageAsync();
            var resume = await Repository.GetAsync(predicate: x => x.Id == id,
                include: x => x.Include(x => x.Translations.Where(t => t.LanguageId == language.Id)));

            if (resume == null || resume.Translations.FirstOrDefault() == null)
                return null!;

            var model = new ResumeViewModel
            {
                Id = resume.Id,
                CandidateId = resume.CandidateId,
                About = resume.Translations.FirstOrDefault().About,
                Skills = resume.Translations.FirstOrDefault().Skills,
                Languages = resume.Translations.FirstOrDefault().Languages
            };

            return model;
        }

        public async Task<ResumeViewModel> GetResume(int resumeId)
        {
            var resume = await GetResumeBase(resumeId);
            resume.Educations = await _educationService.GetEducationModelsOfResume(resumeId);
            resume.Experiences=await _experienceService.GetExperienceModelsOfResume(resumeId);
            resume.PersonalInfo = await _personalInfoService.GetPersonalInfoViewModel(resumeId);

            return resume;
        }

        public async Task<ResumeViewModel> CreateResume(int candidateId)
        {
            var resumeCreateViewModel = new ResumeCreateViewModel
            {
                CandidateId = candidateId,
            };

            var resume = await CreateAsync(resumeCreateViewModel);
            if (resume == null)
                return null!;

            return resume;
        }

        public async Task<Resume> GetResumeWithDetailsAsync(int resumeId, int languageId)
        {
            var resume =await
                Repository.GetAsync(predicate: x => x.Id == resumeId,
                include: x => x.Include(x => x.PersonalInfo)!.ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x=>x.City).ThenInclude(x => x.CityTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x=>x.City).ThenInclude(x=>x.Country).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Experiences).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId==languageId))
                );

            if (resume == null)
                return null!;
            return resume;

        }
    }
}
