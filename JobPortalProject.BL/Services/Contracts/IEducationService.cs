using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IEducationService:ICrudService<Education, EducationViewModel, EducationCreateViewModel, EducationUpdateViewModel>
    {
        public Task<Education> Create(int resumeId, EducationCreateViewModel model);
        public Task<bool> AddEducationToResume(EducationAddViewModel model, int resumeId);
    }
}
