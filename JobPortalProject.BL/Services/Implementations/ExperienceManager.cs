using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
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
        public ExperienceManager(IRepositoryAsync<Experience> repository, IMapper mapper, IExperienceTranslationService experienceTranslationService) : base(repository, mapper)
        {
            _experienceTranslationService = experienceTranslationService;
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
    }
}
