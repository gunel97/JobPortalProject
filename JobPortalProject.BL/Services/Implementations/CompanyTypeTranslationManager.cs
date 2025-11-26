using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyTypeTranslationManager : CrudManager<CompanyTypeTranslation, CompanyTypeTranslationViewModel, CompanyTypeTranslationCreateViewModel, CompanyTypeTranslationUpdateViewModel>
 , ICompanyTypeTranslationService
    {
        public CompanyTypeTranslationManager(IRepositoryAsync<CompanyTypeTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
