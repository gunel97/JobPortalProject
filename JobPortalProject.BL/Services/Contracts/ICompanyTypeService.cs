using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyTypeService : ICrudService<CompanyType, CompanyTypeViewModel, CompanyTypeCreateViewModel, CompanyTypeUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetCompanyTypeSelectListItems(int selectedLanguageId);
        public Task<PagedResultModel<CompanyTypeViewModel>> GetPagedCompanyTypesAsync(CompanyTypeFilterViewModel filter);
        public Task<bool> UpdateCompanyTypeAsync(CompanyTypeUpdateViewModel model);
        public Task<CompanyTypeUpdateViewModel> GetUpdateViewModel(int id);
    }
}
