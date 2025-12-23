using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class EducationManager:CrudManager<Education, EducationViewModel, EducationCreateViewModel, EducationUpdateViewModel>
        , IEducationService
    {
        private readonly IEducationTranslationService _educationTranslationService;
        public EducationManager(IRepositoryAsync<Education> repository, IMapper mapper, IEducationTranslationService educationTranslationService) : base(repository, mapper)
        {
            _educationTranslationService = educationTranslationService;
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
    }
}
