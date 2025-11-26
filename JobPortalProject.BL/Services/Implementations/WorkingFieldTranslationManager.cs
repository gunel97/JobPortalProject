using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class WorkingFieldTranslationManager : CrudManager<WorkingFieldTranslation, WorkingFieldTranslationViewModel, WorkingFieldTranslationCreateViewModel, WorkingFieldTranslationUpdateViewModel>
, IWorkingFieldTranslationService
    {
        public WorkingFieldTranslationManager(IRepositoryAsync<WorkingFieldTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }

    
}
