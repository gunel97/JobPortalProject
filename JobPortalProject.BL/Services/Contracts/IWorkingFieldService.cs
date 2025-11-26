using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IWorkingFieldService : ICrudService<WorkingField, WorkingFieldViewModel, WorkingFieldCreateViewModel, WorkingFieldUpdateViewModel>
    {
    }
}
