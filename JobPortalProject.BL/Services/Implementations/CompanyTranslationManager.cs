using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyTranslationManager : CrudManager<CompanyTranslation, CompanyTranslationViewModel, CompanyTranslationCreateViewModel, CompanyTranslationUpdateViewModel>
 , ICompanyTranslationService
    {
        public CompanyTranslationManager(IRepositoryAsync<CompanyTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
