using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanySocialManager : CrudManager<CompanySocial, CompanySocialViewModel, CompanySocialCreateViewModel, CompanySocialUpdateViewModel>
, ICompanySocialService
    {
        public CompanySocialManager(IRepositoryAsync<CompanySocial> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
