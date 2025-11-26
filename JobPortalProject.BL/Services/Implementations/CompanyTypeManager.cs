using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyTypeManager : CrudManager<CompanyType, CompanyTypeViewModel, CompanyTypeCreateViewModel, CompanyTypeUpdateViewModel>
 , ICompanyTypeService
    {
        public CompanyTypeManager(IRepositoryAsync<CompanyType> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
