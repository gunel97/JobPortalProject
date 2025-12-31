using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CountryManager : CrudManager<Country, CountryViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    , ICountryService
    {
        private readonly ICookieService _cookieService;
        private readonly ILanguageService _languageService;
        private readonly ICountryTranslationService _countryTranslationService;
        private readonly AppDbContext _context;

        public CountryManager(IRepositoryAsync<Country> repository, IMapper mapper, ICookieService cookieService, ILanguageService languageService, ICountryTranslationService countryTranslationService, AppDbContext context) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _languageService = languageService;
            _countryTranslationService = countryTranslationService;
            _context = context;
        }

        public async Task<List<SelectListItem>> GetCountrySelectListItems()
        {
            var language = await _cookieService.GetLanguageAsync();
            var countries = await GetAllAsync(include: x => x.Include(x => x.Translations.Where(t => t.LanguageId == language.Id)));
            var items = new List<SelectListItem>();

            countries.ToList().ForEach(x => items.Add(new SelectListItem(x.Name, x.Id.ToString())));

            return items;
        }

        public async Task<CountryDetailsViewModel> GetDetailsViewModel(int id)
        {
            var languages = await _languageService.GetAllAsync();
            var country = await Repository.GetAsync(predicate: x=>x.Id == id, include: x=>x.Include(x=>x.Translations));
            if (country == null)
                return null!;

            var model = new CountryDetailsViewModel
            {
                Id = id,
                CreatedAt = country.CreatedAt,
                UpdatedAt = country.UpdatedAt,
                Translations = country.Translations.Select(x => new CountryTranslationViewModel
                {
                    Id = x.Id,
                    CountryId = country.Id,
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId).IconUrl,
                    UpdatedAt = x.UpdatedAt
                }).ToList()
            };

            return model;
        }

        public async Task<CountryUpdateViewModel> GetUpdateViewModel(int id)
        {
            var languages = await _languageService.GetAllAsync();
            var country = await Repository.GetAsync(predicate: x=>x.Id== id,
                include: x=>x.Include(x=>x.Translations));
            if (country == null)
                return null!;

            var model = new CountryUpdateViewModel
            {
                Id = country.Id,
                Translations = country.Translations.Select(x => new CountryTranslationUpdateViewModel
                {
                    Id = x.Id,
                    CountryId = country.Id,
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId).IconUrl
                }).ToList()
            };

            return model;
        }

        public async Task<PagedResultModel<CountryViewModel>> GetPagedCountriesAsync(CountryFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            Expression<Func<Country, bool>> predicate = BuildPredicate(filter, language.Id);
            Func<IQueryable<Country>, IOrderedQueryable<Country>> orderBy = BuildOrderBy(filter, language.Id);
            var pagedCountries = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: orderBy,
                include: x => x
                .Include(x => x.Translations.Where(t => t.LanguageId == language.Id))
                .Include(x=>x.Cities).ThenInclude(x=>x.Addresses).ThenInclude(x=>x.Company)!
                .Include(x=>x.Cities).ThenInclude(x=>x.Addresses).ThenInclude(x=>x.PersonalInfo)!,
                index: filter.Index, 
                size: filter.Size);

            var countryViewModels = new List<CountryViewModel>();
            foreach (var item in pagedCountries.Items)
            {
                var model = Mapper.Map<CountryViewModel>(item);

                model.CityCount = item.Cities.Count();
                var companyAddressIds = new List<int>();
                var candidateAddressIds = new List<int>();

                var cities = item.Cities;

                foreach (var city in cities)
                {
                    foreach (var address in city.Addresses)
                    {
                        if (address.Company != null)
                            companyAddressIds.Add(address.Company.Id);
                        if(address.PersonalInfo!=null)
                            candidateAddressIds.Add(address.PersonalInfo.Id);
                    }
                }
                model.CompanyAddressCount=companyAddressIds.Count();
                model.CandidateCount=candidateAddressIds.Count();
                model.CompanyCount = companyAddressIds.Distinct().ToList().Count();

                countryViewModels.Add(model);
            }

            var pagedCountryModels = new PagedResultModel<CountryViewModel>
            {
                Items = countryViewModels,
                Index = pagedCountries.Index,
                Size = pagedCountries.Size,
                Count = pagedCountries.Count,
                Pages = pagedCountries.Pages,
            };

            return pagedCountryModels;
        }

        private Func<IQueryable<Country>, IOrderedQueryable<Country>> BuildOrderBy(CountryFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "createdat";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            // Handle compound sort keys (e.g. "name_desc")
            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                if (parts.Length > 1) sortOrder = parts[1];
            }

            return queryable =>
            {
                IOrderedQueryable<Country> ordered;

                switch (sortBy)
                {
                    case "name":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Translations
                                            .Where(t => t.LanguageId == languageId)
                                            .Select(t => t.Name)
                                            .FirstOrDefault())
                            : queryable.OrderByDescending(x => x.Translations
                                            .Where(t => t.LanguageId == languageId)
                                            .Select(t => t.Name)
                                            .FirstOrDefault());
                        break;

                    // 1. City Count: Count cities linked to the country
                    case "citycount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => _context.Cities.Count(c => c.CountryId == x.Id))
                            : queryable.OrderByDescending(x => _context.Cities.Count(c => c.CountryId == x.Id));
                        break;

                    // 2. Company Address Count: Count ALL addresses in this country that have a Company
                    case "companyaddresscount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => _context.Addresses
                                .Count(a => a.City!.CountryId == x.Id && a.CompanyId != null))
                            : queryable.OrderByDescending(x => _context.Addresses
                                .Count(a => a.City!.CountryId == x.Id && a.CompanyId != null));
                        break;

                    // 3. Company Count (Distinct): Count UNIQUE CompanyIds in this country
                    case "companycount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => _context.Addresses
                                .Where(a => a.City!.CountryId == x.Id && a.CompanyId != null)
                                .Select(a => a.CompanyId)
                                .Distinct()
                                .Count())
                            : queryable.OrderByDescending(x => _context.Addresses
                                .Where(a => a.City!.CountryId == x.Id && a.CompanyId != null)
                                .Select(a => a.CompanyId)
                                .Distinct()
                                .Count());
                        break;
                        
                    // 4. Candidate Count: Count addresses in this country that have PersonalInfo
                    case "candidatecount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => _context.Addresses
                                .Count(a => a.City!.CountryId == x.Id && a.PersonalInfo != null))
                            : queryable.OrderByDescending(x => _context.Addresses
                                .Count(a => a.City!.CountryId == x.Id && a.PersonalInfo != null));
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

        private Expression<Func<Country, bool>> BuildPredicate(CountryFilterViewModel filter, int languageId)
        {
            Expression<Func<Country, bool>> predicate = x => string.IsNullOrEmpty(filter.SearchTerm) ||
                x.Translations.Any(t => t.LanguageId == languageId && (t.Name.Contains(filter.SearchTerm)));

            return predicate;
        }
    }


}
