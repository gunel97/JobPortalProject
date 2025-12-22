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
        public EducationManager(IRepositoryAsync<Education> repository, IMapper mapper):base(repository, mapper) { }

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
    }
}
