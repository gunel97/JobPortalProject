using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class EducationManager:CrudManager<Education, EducationViewModel, EducationCreateViewModel, EducationUpdateViewModel>
        , IEducationService
    {
        public EducationManager(IRepositoryAsync<Education> repository, IMapper mapper):base(repository, mapper) { }
    }
}
