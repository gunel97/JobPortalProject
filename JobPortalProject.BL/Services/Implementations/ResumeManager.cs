using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ResumeManager:CrudManager<Resume, ResumeViewModel, ResumeCreateViewModel, ResumeUpdateViewModel>
        , IResumeService
    {
        public ResumeManager(IRepositoryAsync<Resume> repository, IMapper mapper):base(repository, mapper) { }
    }
}
