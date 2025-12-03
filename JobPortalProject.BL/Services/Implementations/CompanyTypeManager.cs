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
        private readonly ICookieService _cookieService;
        public CompanyTypeManager(IRepositoryAsync<CompanyType> repository, IMapper mapper, ICookieService cookieService) : base(repository, mapper)
        {
            _cookieService = cookieService;
        }

        public async Task<List<SelectListItem>> GetCompanyTypeSelectListItems()
        {
            var language = await _cookieService.GetLanguageAsync();
            var companyTypesSelectListItems = new List<SelectListItem>();
            var companyTypes = await Repository.GetAllAsync(include: 
                x=>x.Include(c=>c.CompanyTypeTranslations.Where(ct=>ct.LanguageId==language.Id)));
            var companyTypeViewModelsList = companyTypes.Select(
                x=>Mapper.Map<CompanyTypeViewModel>(x)).ToList();

            companyTypeViewModelsList.ForEach(x => companyTypesSelectListItems.Add(
                new SelectListItem(x.Name, x.Id.ToString())));

            return companyTypesSelectListItems;
        }
    }


}
