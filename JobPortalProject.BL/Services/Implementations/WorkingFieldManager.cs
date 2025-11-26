using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class WorkingFieldManager : CrudManager<WorkingField, WorkingFieldViewModel, WorkingFieldCreateViewModel, WorkingFieldUpdateViewModel>
, IWorkingFieldService
    {
        public WorkingFieldManager(IRepositoryAsync<WorkingField> repository, IMapper mapper) : base(repository, mapper) { }
    }

    
}
