using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CityManager : CrudManager<City, CityViewModel, CityCreateViewModel, CityUpdateViewModel>
, ICityService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;
        private readonly ICityTranslationService _cityTranslationService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;
        private readonly ICountryService _countryService;

        public CityManager(IRepositoryAsync<City> repository, IMapper mapper, ILanguageService languageService, ICookieService cookieService, ICityTranslationService cityTranslationService, ICloudinaryService cloudinaryService, FileService fileService, ICountryService countryService) : base(repository, mapper)
        {
            _languageService = languageService;
            _cookieService = cookieService;
            _cityTranslationService = cityTranslationService;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
            _countryService = countryService;
        }

        public override async Task<bool> UpdateAsync(int id, CityUpdateViewModel model)
        {
            var city = await Repository.GetAsync(predicate: x=>x.Id== id, include: x=>x.Include(x=>x.CityTranslations));
            if (city == null)
                return false;

            if (model.ImageFile != null)
            {
                if (!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));

                var result = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.JobCategoryImagePath);
                if (result.Success)
                {
                    if (city.CoverPhotoPublicId != null)
                    {
                        await _cloudinaryService.DeleteImageAsync(city.CoverPhotoPublicId);
                    }
                    model.CoverPhotoPublicId = result.PublicId;
                    model.CoverPhotoUrl = result.Url;
                }
            }
            else
            {
                model.CoverPhotoUrl = city.CoverPhotoUrl;
                model.CoverPhotoPublicId=city.CoverPhotoPublicId;
            }
                return await base.UpdateAsync(id, model);
        }

        public async Task<CityUpdateViewModel> GetUpdateViewModel(int id)
        {
            var countries = await _countryService.GetCountrySelectListItems();
            var languages = await _languageService.GetAllAsync();
            var city = await Repository.GetAsync(predicate: x => x.Id == id, include: x => x.Include(x => x.CityTranslations));
            var updateModel = Mapper.Map<CityUpdateViewModel>(city);
            updateModel.CountryItems = countries;

            foreach(var translation in updateModel.CityTranslations)
            {
                translation.LanguageIcon = languages.FirstOrDefault(x => x.Id == translation.LanguageId).IconUrl;
            }

            return updateModel;
        }

        public async Task<CityDetailsViewModel> GetDetailsViewModel(int id)
        {
            var city = await Repository.GetAsync(predicate: x => x.Id == id,
                include: x => x.Include(x => x.CityTranslations)
                .Include(x => x.Country).ThenInclude(x => x.Translations));
            var languages = await _languageService.GetAllAsync();
            if (city == null)
                return null!;

            var model = new CityDetailsViewModel
            {
                Id = city.Id,
                CoverPhotoUrl = city.CoverPhotoUrl,
                CreatedAt = city.CreatedAt,
                UpdatedAt = city.UpdatedAt,
                CityTranslations = city.CityTranslations.Select(x => new CityTranslationViewModel
                {
                    Id = x.Id,
                    CityId = city.Id,
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    UpdatedAt = x.UpdatedAt,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId).IconUrl
                }).ToList()
            };

            return model;
        }

        public override async Task<CityViewModel> CreateAsync(CityCreateViewModel model)
        {
            if (model.ImageFile != null)
            {
                if (!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));

                var result = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.CityImagePath);

                if (result.Success)
                {
                    model.CoverPhotoUrl = result.Url;
                    model.CoverPhotoPublicId = result.PublicId;
                }
            }

            return await base.CreateAsync(model);
        }

        public async Task<PagedResultModel<CityViewModel>> GetPagedCitiesAsync(CityFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            Expression<Func<City, bool>> predicate = BuildPredicate(filter, language.Id);
            Func<IQueryable<City>, IOrderedQueryable<City>> orderBy = BuildOrderBy(filter, language.Id);
            var pagedCities = await Repository.GetPagedListAsync(
                predicate: predicate,
                orderBy: orderBy,
                include: x => x
                .Include(x => x.CityTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Addresses).ThenInclude(x => x.Company)!
                .Include(x => x.Addresses).ThenInclude(x => x.PersonalInfo)!
                .Include(x=>x.Country).ThenInclude(x=>x.Translations.Where(x=>x.LanguageId==language.Id)),
                index: filter.Index,
                size: filter.Size);

            var cityViewModels = new List<CityViewModel>();
            foreach (var item in pagedCities.Items)
            {
                var model = Mapper.Map<CityViewModel>(item);

                model.AddressCount = item.Addresses.Count();
                var companyAddressIds = new List<int>();
                var candidateAddressIds = new List<int>();

                foreach(var address in item.Addresses)
                {
                    if (address.Company != null && address.CompanyId != 0)
                        companyAddressIds.Add(address.Company.Id);
                    if (address.PersonalInfo != null)
                        candidateAddressIds.Add(address.PersonalInfo.Id);
                }

                model.AddressCount = item.Addresses.Count();
                model.CompanyAddressCount = companyAddressIds.Count();
                model.CandidateCount=candidateAddressIds.Count();
                model.CompanyCount = companyAddressIds.Distinct().ToList().Count();

                cityViewModels.Add(model);
            }

            var pagedCityModels = new PagedResultModel<CityViewModel>
            {
                Items = cityViewModels,
                Index = pagedCities.Index,
                Size = pagedCities.Size,
                Count = pagedCities.Count,
                Pages = pagedCities.Pages,
            };

            return pagedCityModels;
        }

        public async Task<List<SelectListItem>> GetCitySelectListItemsWithCountry(int selectedLanguageId)
        {
            var citySelectListItems = new List<SelectListItem>();
            var cities = await Repository.GetAllAsync(include: x => x.
                Include(c => c.CityTranslations.Where(t => t.LanguageId == selectedLanguageId)).
                Include(c => c.Country).ThenInclude(ct => ct.Translations.Where(t => t.LanguageId == selectedLanguageId)));                

            var cityViewModels = cities.Select(
                x => Mapper.Map<CityViewModel>(x)).ToList();
            cityViewModels.ForEach(x => citySelectListItems.Add(
                new SelectListItem(x.Name + ", "+ x.Country!.Name, x.Id.ToString())));

            return citySelectListItems;
        }

        private Expression<Func<City, bool>> BuildPredicate(CityFilterViewModel filter, int languageId)
        {
            Expression<Func<City, bool>> predicate = x => 
                (string.IsNullOrEmpty(filter.SearchTerm) ||
                x.CityTranslations.Any(t => t.LanguageId == languageId && t.Name.Contains(filter.SearchTerm))) &&
                ((!filter.CountryId.HasValue || x.CountryId == filter.CountryId));

            return predicate;
        }

        private Func<IQueryable<City>, IOrderedQueryable<City>> BuildOrderBy(CityFilterViewModel filter, int languageId)
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
                IOrderedQueryable<City> ordered;

                switch (sortBy)
                {
                    case "name":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.CityTranslations
                                                    .Where(t => t.LanguageId == languageId)
                                                    .Select(t => t.Name)
                                                    .FirstOrDefault())
                            : queryable.OrderByDescending(x => x.CityTranslations
                                                    .Where(t => t.LanguageId == languageId)
                                                    .Select(t => t.Name)
                                                    .FirstOrDefault());
                        break;

                    // 1. Address Count: Total addresses in the city
                    case "addresscount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Addresses.Count)
                            : queryable.OrderByDescending(x => x.Addresses.Count);
                        break;

                    // 2. Company Count (Distinct): Count unique companies in the city
                    case "companycount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Addresses
                                                    .Where(a => a.CompanyId != null)
                                                    .Select(a => a.CompanyId)
                                                    .Distinct()
                                                    .Count())
                            : queryable.OrderByDescending(x => x.Addresses
                                                    .Where(a => a.CompanyId != null)
                                                    .Select(a => a.CompanyId)
                                                    .Distinct()
                                                    .Count());
                        break;

                    // 3. Company Address Count: Count total addresses linked to companies
                    case "companyaddresscount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Addresses.Count(a => a.CompanyId != null))
                            : queryable.OrderByDescending(x => x.Addresses.Count(a => a.CompanyId != null));
                        break;

                    // 4. Candidate Count: Count addresses with PersonalInfo
                    case "candidatecount":
                        ordered = sortOrder == "asc"
                            ? queryable.OrderBy(x => x.Addresses.Count(a => a.PersonalInfo != null))
                            : queryable.OrderByDescending(x => x.Addresses.Count(a => a.PersonalInfo != null));
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
    }


}
