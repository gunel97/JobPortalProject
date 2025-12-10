using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyTypeManager : CrudManager<CompanyType, CompanyTypeViewModel, CompanyTypeCreateViewModel, CompanyTypeUpdateViewModel>
 , ICompanyTypeService
    {
        public CompanyTypeManager(IRepositoryAsync<CompanyType> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<List<SelectListItem>> GetCompanyTypeSelectListItems(int selectedLanguageId)
        {
            var companyTypesSelectListItems = new List<SelectListItem>();

            var companyTypes = await Repository.GetAllAsync(include: 
                x=>x.Include(c=>c.CompanyTypeTranslations.Where(ct=>ct.LanguageId==selectedLanguageId)));
            var companyTypeViewModelsList = companyTypes.Select(
                x=>Mapper.Map<CompanyTypeViewModel>(x)).ToList();

            companyTypeViewModelsList.ForEach(x => companyTypesSelectListItems.Add(
                new SelectListItem(x.Name, x.Id.ToString())));


            return companyTypesSelectListItems;
        }
    }


}
