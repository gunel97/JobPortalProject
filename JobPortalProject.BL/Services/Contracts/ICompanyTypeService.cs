using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyTypeService : ICrudService<CompanyType, CompanyTypeViewModel, CompanyTypeCreateViewModel, CompanyTypeUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetCompanyTypeSelectListItems();
    }
}
