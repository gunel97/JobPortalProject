using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IWorkingFieldTranslationService : ICrudService<WorkingFieldTranslation, WorkingFieldTranslationViewModel, WorkingFieldTranslationCreateViewModel, WorkingFieldTranslationUpdateViewModel>
    {
    }
}
