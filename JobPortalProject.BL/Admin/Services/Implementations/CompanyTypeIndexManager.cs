using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class CompanyTypeIndexManager:ICompanyTypeIndexService
    {
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public CompanyTypeIndexManager(ICompanyTypeService companyTypeService, ILanguageService languageService, ICookieService cookieService)
        {
            _companyTypeService = companyTypeService;
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<CompanyTypePagedIndexViewModel> GetPagedCompanyTypeIndexModel(CompanyTypeFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new CompanyTypeFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedCompanyTypes = await _companyTypeService.GetPagedCompanyTypesAsync(filter);

            var model = new CompanyTypePagedIndexViewModel
            {
                Languages = languages.ToList(),
                Filter=filter,
                CompanyTypes = pagedCompanyTypes
            };

            return model;
        }
    }
}
