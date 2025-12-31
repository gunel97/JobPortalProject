using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyTypeManager : CrudManager<CompanyType, CompanyTypeViewModel, CompanyTypeCreateViewModel, CompanyTypeUpdateViewModel>
 , ICompanyTypeService
    {
        private readonly ICookieService _cookieService;
        private readonly ILanguageService _languageService;
        private readonly ICompanyTypeTranslationService _companyTypeTranslationService;


        public CompanyTypeManager(IRepositoryAsync<CompanyType> repository, IMapper mapper, ICookieService cookieService, ILanguageService languageService, ICompanyTypeTranslationService companyTypeTranslationService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _languageService = languageService;
            _companyTypeTranslationService = companyTypeTranslationService;
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

        public async Task<CompanyTypeDetailsViewModel> GetDetailsViewModel(int id)
        {
            var type = await Repository.GetAsync(predicate: x => x.Id == id,
                include: x => x.Include(x => x.CompanyTypeTranslations)
                .Include(x=>x.Companies));
            var languages = await _languageService.GetAllAsync();
            if (type == null)
                return null!;

            var model = new CompanyTypeDetailsViewModel
            {
                Id = id,
                CreatedAt = type.CreatedAt,
                UpdatedAt= type.UpdatedAt,
                CompanyCount=type.Companies.Count(),
                Translations = type.CompanyTypeTranslations.Select(x => new CompanyTypeTranslationViewModel
                {
                    Id = x.Id,
                    CompanyTypeId=type.Id,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId).IconUrl,
                    Name=x.Name,
                    UpdatedAt=x.UpdatedAt,
                }).ToList()
            };

            return model;
        }

        public async Task<CompanyTypeUpdateViewModel> GetUpdateViewModel(int id)
        {
            var type= await Repository.GetAsync(predicate: x=>x.Id==id,
                include: x=>x.Include(x=>x.CompanyTypeTranslations));
            if (type == null)
                return null!;
            var languages = await _languageService.GetAllAsync();

            var model = new CompanyTypeUpdateViewModel
            {
                Id = type.Id,
                Translations = type.CompanyTypeTranslations.Select(x => new CompanyTypeTranslationUpdateViewModel
                {
                    Id = x.Id,
                    CompanyTypeId = type.Id,
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId)!=null ? 
                    languages.FirstOrDefault(l => l.Id == x.LanguageId).IconUrl : ""                    
                }).ToList()
            };

            return model;
        }

        public async Task<bool> UpdateCompanyTypeAsync(CompanyTypeUpdateViewModel model)
        {
            var type = await Repository.GetAsync(predicate: x => x.Id == model.Id,
                include: x => x.Include(c => c.CompanyTypeTranslations));
            if(type == null) 
                return false;

            foreach(var translation in model.Translations)
            {
                translation.CompanyTypeId= type.Id;
                var translationResult = await _companyTypeTranslationService.UpdateAsync(translation.Id, translation);
                if (!translationResult) return false;
            }

           var result =  await Repository.UpdateAsync(type);
            if (result == null)
                return false;

            return true;
        }

        public async Task<PagedResultModel<CompanyTypeViewModel>> GetPagedCompanyTypesAsync(CompanyTypeFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            Expression<Func<CompanyType, bool>> predicate = BuildPredicate(filter, language.Id);
            Func<IQueryable<CompanyType>, IOrderedQueryable<CompanyType>> orderBy = BuildOrderBy(filter, language.Id);

            var pagedCompanyTypes = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: orderBy,
                include: x => x.
                Include(x => x.Companies).
                Include(x => x.CompanyTypeTranslations.Where(t => t.LanguageId == language.Id)),
                index: filter.Index, size: filter.Size
                );

            var companyTypeModels = new List<CompanyTypeViewModel>();
            foreach (var item in pagedCompanyTypes.Items)
            {
                var model = Mapper.Map<CompanyTypeViewModel>(item);
                companyTypeModels.Add(model);
            }

            var pagedCompanyTypeModels = new PagedResultModel<CompanyTypeViewModel>
            {
                Items = companyTypeModels,
                Index = pagedCompanyTypes.Index,
                Size = pagedCompanyTypes.Size,
                Count = pagedCompanyTypes.Count,
                Pages = pagedCompanyTypes.Pages,
            };

            return pagedCompanyTypeModels;
        }

        private Func<IQueryable<CompanyType>, IOrderedQueryable<CompanyType>> BuildOrderBy(CompanyTypeFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower().Trim() ?? "createdat";
            var sortOrder = filter.SortOrder?.ToLower().Trim() ?? "desc";

            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                if (parts.Length > 1) sortOrder = parts[1];
            }

            return queryable =>
            {
                IOrderedQueryable<CompanyType> ordered;

                switch (sortBy)
                {
                    case "name":
                        if (sortOrder == "asc")
                        {
                            ordered = queryable.OrderBy(x => x.CompanyTypeTranslations
                                                .Where(t => t.LanguageId == languageId)
                                                .Select(t => t.Name)
                                                .FirstOrDefault());
                        }
                        else
                        {
                            ordered = queryable.OrderByDescending(x => x.CompanyTypeTranslations
                                                .Where(t => t.LanguageId == languageId)
                                                .Select(t => t.Name)
                                                .FirstOrDefault());
                        }
                        break;
                    case "companycount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Companies.Count())
                            : queryable.OrderByDescending(x => x.Companies.Count());
                        break;
                    case "createdat":
                    default:
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.CreatedAt)
                            : queryable.OrderByDescending(x => x.CreatedAt);
                        break;
                }

                return ordered;
            };
        }

        private Expression<Func<CompanyType, bool>> BuildPredicate(CompanyTypeFilterViewModel filter, int languageId)
        {
            Expression<Func<CompanyType, bool>> predicate = x => string.IsNullOrEmpty(filter.SearchTerm) ||
                x.CompanyTypeTranslations.Any(t => t.LanguageId == languageId && (t.Name.Contains(filter.SearchTerm)));

            return predicate;
        }
    }


}
