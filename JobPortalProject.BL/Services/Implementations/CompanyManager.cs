using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MimeKit.Cryptography;
using System.Linq.Expressions;
using System.Security.Claims;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CompanyManager : CrudManager<Company, CompanyViewModel, CompanyCreateViewModel, CompanyUpdateViewModel>
 , ICompanyService
    {
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICookieService _cookieService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanySocialService _companySocialService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;
        private readonly IWorkingFieldService _workingFieldService;
        private readonly ICompanyTranslationService _companyTranslationService;
        private readonly IAddressService _addressService;
        private readonly ILanguageService _languageService;
        private readonly ICityService _cityService;
        private readonly ISocialMediaService _socialMediaService;

        public CompanyManager(IRepositoryAsync<Company> repository, IMapper mapper, ICompanyTypeService companyTypeService, ICookieService cookieService, IHttpContextAccessor httpContextAccessor, ICompanySocialService companySocialService, ICloudinaryService cloudinaryService, FileService fileService, ICompanyTranslationService translationService, IWorkingFieldService workingFieldService, IAddressService addressService, ILanguageService languageService, ICityService cityService, ISocialMediaService socialMediaService) : base(repository, mapper)
        {
            _companyTypeService = companyTypeService;
            _cookieService = cookieService;
            _httpContextAccessor = httpContextAccessor;
            _companySocialService = companySocialService;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
            _companyTranslationService = translationService;
            _workingFieldService = workingFieldService;
            _addressService = addressService;
            _languageService = languageService;
            _cityService = cityService;
            _socialMediaService = socialMediaService;
        }

        public async Task<List<CompanyViewModel>> GetAllCompaniesAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();
            int languageId = language.Id;

            var companies = await Repository.GetAllAsync(
                predicate: x => !x.IsDeleted,
                include: x => x.Include(x=>x.CompanyTranslations)
                .Include(x=>x.Jobs).ThenInclude(x=>x.JobTranslations));

            var companyViewModels = new List<CompanyViewModel>();

            foreach (var company in companies)
            {
                if (company.CompanyTranslations.Count() != 0)
                {
                    var model = Mapper.Map<CompanyViewModel>(company);
                    foreach (var translation in company.CompanyTranslations)
                    {
                        foreach (var lang in languages)
                        {
                            if (translation.LanguageId == lang.Id)
                                model.ReadyLanguages.Add(lang);
                        }
                    }

                    foreach (var lang in languages)
                    {
                        if (!model.ReadyLanguages.Contains(lang))
                            model.EmptyLanguages.Add(lang);
                    }

                    model.ActiveJobCount = 0;
                    foreach (var job in company.Jobs)
                    {
                        if (job.JobTranslations.FirstOrDefault(x => x.LanguageId == language.Id) != null && job.IsActive
                           && !job.IsDeleted && job.ExpirationDate > DateTime.UtcNow)
                            model.ActiveJobCount++;
                    }


                    foreach (var readyLang in model.ReadyLanguages)
                    {
                        if (readyLang.Id == language.Id)
                            companyViewModels.Add(model);
                    }
                }
            }

            return companyViewModels;
        }

        public async Task<CheckoutViewModel> GetCheckoutViewModelAsync()
        {
            
            var company = await GetCompanyOfUser();
            var language = await _cookieService.GetLanguageAsync();

            if (company == null || company.AppUser==null)
                return null!;

            var model = new CheckoutViewModel
            {
                FullName = company.AppUser.FirstName + " " + company.AppUser.LastName,
                CompanyName = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == language.Id).Name,
                Phone=company.PrimaryPhone,
                Email=company.CompanyEmail,
            };

            return model;
        }

        public async Task<int> GetCompanyIdOfUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (company == null)
            {
                return 0;
            }

            else
                return company.Id;
        }

        public async Task<Company> GetCompanyOfUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x.Include(x => x.CompanyTranslations)
                .Include(x => x.WorkingFields).ThenInclude(x => x.Translations)
                .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations)
                .Include(x=>x.AppUser));

            return company!;
        }

        public async Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var selectedLanguageId = language.Id;
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);
            var languages = await _languageService.GetAllAsync();

            if (existedCompany == null)
                return null!;

            var company = await Repository.GetAsync(
                                           predicate: x => !x.IsDeleted && x.Id == existedCompany.Id,
                                           include: x => x
                                           .Include(ct => ct.CompanyTranslations!).ThenInclude(x => x.Language)
                                           .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations)
                                           .Include(t => t.CompanyType!).ThenInclude(ct => ct.CompanyTypeTranslations!.Where(x => x.LanguageId == selectedLanguageId))
                                           .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations));

            if (company == null)
                return null!;

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == existedCompany.Id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems(selectedLanguageId);
            var citiesList = await _cityService.GetCitySelectListItemsWithCountry(selectedLanguageId);
            var addressesList = await _addressService.GetAddressSelectListItems(existedCompany.Id, selectedLanguageId);
            var socialMediasList = await _socialMediaService.GetSocialMediaListItems();
            var emptyLanguages = new List<LanguageViewModel>();

            foreach (var item in languages)
            {
                if (!company.CompanyTranslations.Any(x => x.LanguageId == item.Id))
                    emptyLanguages.Add(item);
            }

            var companyUpdateViewModel = new CompanyUpdateViewModel
            {
                Id = company!.Id,
                SelectedUpdateLanguageId = selectedLanguageId,
                CompanySize = company.CompanySize,
                CompanyEmail = company.CompanyEmail,
                PrimaryPhone = company.PrimaryPhone,
                SecondaryPhone = company.SecondaryPhone,
                CoverPhotoUrl = company.CoverPhotoUrl,
                LogoUrl = company.LogoUrl,
                CompanyTypeId = company.CompanyTypeId,
                CompanyTypeList = companyTypeSelectListItems,
                AddressesOfCompany = addressesList,
                CitiesList = citiesList,
                SocialMediasList = socialMediasList,
                EmptyLanguages = emptyLanguages,
                CompanyTranslations = company.CompanyTranslations.Select(x => new CompanyTranslationUpdateViewModel
                {
                    TranslationId = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    LanguageId = x.LanguageId,
                    CompanyId = company.Id,
                }).ToList(),
                CompanySocialUpdateViewModels = companySocials.Select(x => new CompanySocialUpdateViewModel
                {
                    Id = x.Id,
                    CompanyId = x.Id,
                    SocialMediaId = x.SocialMediaId,
                    AddressUrl = x.AddressUrl,
                    Title = x.SocialMedia!.Title,
                    IconUrl = x.SocialMedia.IconUrl
                }).ToList()
            };

            if(company.Addresses.Any())
            {
                foreach(var address in company.Addresses)
                {
                    if (address.IsMainAddress)
                        companyUpdateViewModel.MainAddressId = address.Id;
                }
            }

            foreach (var translation in companyUpdateViewModel.CompanyTranslations)
            {
                translation.LanguageIcon = languages.FirstOrDefault(x => x.Id == translation.LanguageId)!.IconUrl;
            }

            return companyUpdateViewModel;
        }

        public async Task<bool> AddTranslationToExistingCompany(AddTranslationToExistedCompanyViewModel model)
        {
            var company = await GetCompanyOfUser();
            if (company == null)
                return false;

            foreach (var address in model.addressTranslationCreateModels)
            {
                var resultAddress = await _addressService.AddTranslationToExistingAddress(address);
                if (!resultAddress)
                    return false;
            }

            foreach (var workingField in model.workingFieldTranslationCreateModels)
            {
                var resultField = await _workingFieldService.AddTranslationToExistingWorkingField(workingField);
                if (!resultField)
                    return false;
            }

            company.CompanyTranslations.Add(new CompanyTranslation
            {
                LanguageId = model.translationModel.LanguageId,
                CompanyId = company.Id,
                Name = model.translationModel.Name!,
                Description = model.translationModel.Description
            });

            var result = await Repository.UpdateAsync(company);
            if (result == null)
                return false;

            return true;
        }

        public async Task<CompanyTranslationEditPageViewModel> GetCompanyTranslationEditPageAsync(int languageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId && !x.IsDeleted,
                                           include: x => x
                                           .Include(ct => ct.CompanyTranslations!)
                                           .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations)
                                           .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations));

            if (existedCompany == null)
                return null!;

            var translationUpdateViewModels = existedCompany.CompanyTranslations.Select(x => new CompanyTranslationUpdateViewModel
            {
                TranslationId = x.Id,
                Name = x.Name,
                Description = x.Description,
                LanguageId = x.LanguageId,
                CompanyId = existedCompany.Id,
            }).ToList();

            var workingFieldUpdateViewModels = await _workingFieldService.GetUpdateViewModelAsync(existedCompany.Id, languageId);
            var addressUpdateViewModels = await _addressService.GetAddressUpdateViewModels(existedCompany.Id, languageId);

            var model = new CompanyTranslationEditPageViewModel
            {
                LangaugeId = languageId,
                CompanyTranslationUpdateViewModel = translationUpdateViewModels.FirstOrDefault(x => x.LanguageId == languageId),
                WorkingFieldUpdateViewModels = workingFieldUpdateViewModels,
                AddressUpdateViewModels = addressUpdateViewModels,
            };

            return model;
        }

        public async Task<AddTranslationToExistedCompanyViewModel> GetAddTranslationToExistedCompanyViewModel(int languageId)
        {
            var company = await GetCompanyOfUser();
            if (company == null)
                return null!;

            var images = new List<string>();
            var translation = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId != languageId);
            var workingFields = company.WorkingFields.Where(x => !x.Translations.Any(t => t.LanguageId == languageId)).ToList();
            var addresses = company.Addresses.Where(x => !x.AddressTranslations.Any(t => t.LanguageId == languageId)).ToList();

            if (translation == null)
                return null!;

            var model = new AddTranslationToExistedCompanyViewModel
            {
                LanguageId = languageId,
                CompanyId = company.Id,
                translationModel = new CompanyTranslationCreateViewModel
                {
                    CompanyId = company.Id,
                    LanguageId = languageId,
                }
            };

            var cities = (await _cityService.GetAllAsync(include:
                x => x.Include(c => c.CityTranslations.Where(t => t.LanguageId == languageId))
                .Include(c => c.Country).ThenInclude(ct => ct.Translations.Where(t => t.LanguageId == languageId)))).ToList();

            if (addresses.Any() || addresses != null)
            {
                model.addressTranslationCreateModels = addresses.Select(x => new AddressTranslationCreateViewModel
                {
                    AddressId = x.Id,
                    LanguageId = languageId,
                    ExistingAddress = cities.FirstOrDefault(c => c.Id == x.CityId)!.Name + ", " + cities.FirstOrDefault(c => c.Id == x.CityId)!.Country!.Name
                    + ", " + x.AddressTranslations.FirstOrDefault()!.Street,
                }).ToList();
            }

            if (workingFields.Any() || workingFields != null)
            {
                model.workingFieldTranslationCreateModels = workingFields.Select(x => new WorkingFieldTranslationCreateViewModel
                {
                    WorkingFieldId = x.Id,
                    LanguageId = languageId,
                    IconUrl = x.IconUrl,
                    ExistingFieldName = x.Translations.FirstOrDefault()!.Name
                }).ToList();
            }


            return model;
        }

        public async Task<bool> UpdateCompanyTranslation(CompanyTranslationEditPageViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId && !x.IsDeleted,
                                           include: x => x
                                           .Include(ct => ct.CompanyTranslations!)
                                           .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations)
                                           .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations));

            if (existedCompany == null)
                return false;

            var translationUpdateModel = model.CompanyTranslationUpdateViewModel;
            var addressUpdateModels = model.AddressUpdateViewModels;
            var workingFieldUpdateModels = model.WorkingFieldUpdateViewModels;
            var selectedLanguageId = model.LangaugeId;

            foreach (var addressModel in addressUpdateModels)
            {
                await _addressService.UpdateAddressAsync(selectedLanguageId, addressModel.Id, addressModel);
            }

            foreach (var workingFieldModel in workingFieldUpdateModels)
            {
                await _workingFieldService.UpdateWorkingFieldAsync(selectedLanguageId, workingFieldModel.WorkingFieldId, workingFieldModel);
            }

            translationUpdateModel.LanguageId = selectedLanguageId;
            translationUpdateModel.CompanyId = existedCompany.Id;

            var result = await _companyTranslationService.UpdateAsync(translationUpdateModel.TranslationId, translationUpdateModel);

            return result;
        }

        public override async Task<bool> UpdateAsync(int selectedLanguageId, CompanyUpdateViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);
            if (existedCompany == null)
                return false;


            var addressesOfCompany = await _addressService.GetByCompanyIdAsync(existedCompany.Id);
            var currentMainAddressOfCompany = addressesOfCompany.FirstOrDefault(x => x.IsMainAddress);
            var newMainAddressOfCompany = addressesOfCompany.FirstOrDefault(x => x.Id == model.MainAddressId);

            if (currentMainAddressOfCompany!.Id != model.MainAddressId)
            {
                if (newMainAddressOfCompany == null || newMainAddressOfCompany.CompanyId != existedCompany.Id)
                {
                    return false;
                }
                foreach (var address in addressesOfCompany)
                {
                    address.IsMainAddress = false;
                }
                newMainAddressOfCompany.IsMainAddress = true;
            }
            existedCompany = Mapper.Map(model, existedCompany);

            if (model.CoverPhotoFile != null)
            {
                if (!_fileService.IsImageFile(model.CoverPhotoFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.CoverPhotoFile));

                var resultCover = await _cloudinaryService.UploadImageAsync(model.CoverPhotoFile, FilePathConstants.CompanyImagePath);

                if (resultCover.Success)
                {
                    if (existedCompany.CoverPhotoPublicId != null)
                    {
                        var deleteResult = await _cloudinaryService.DeleteImageAsync(existedCompany.CoverPhotoPublicId!);
                    }
                    existedCompany.CoverPhotoUrl = resultCover.Url;
                    existedCompany.CoverPhotoPublicId = resultCover.PublicId;
                }
            }
            else
            {
                existedCompany.CoverPhotoUrl = model.CoverPhotoUrl;
            }

            if (model.LogoFile != null)
            {
                if (!_fileService.IsImageFile(model.LogoFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.LogoFile));

                var resultLogo = await _cloudinaryService.UploadImageAsync(model.LogoFile, FilePathConstants.CompanyImagePath);

                if (resultLogo.Success)
                {
                    if (existedCompany.LogoPublicId != null)
                    {
                        var deleteResult = await _cloudinaryService.DeleteImageAsync(existedCompany.LogoPublicId!);
                    }
                    existedCompany.LogoUrl = resultLogo.Url;
                    existedCompany.LogoPublicId = resultLogo.PublicId;
                }
            }
            else
            {
                existedCompany.LogoUrl = model.LogoUrl;
            }

            foreach (var companySocialModel in model.CompanySocialUpdateViewModels)
            {
                var companySocial = await _companySocialService.GetAsync(predicate: x => x.Id == companySocialModel.Id);
                companySocialModel.CompanyId = existedCompany.Id;
                companySocialModel.SocialMediaId = companySocial.SocialMediaId;
                await _companySocialService.UpdateAsync(companySocialModel.Id, companySocialModel);
            }

            existedCompany.IsAccountApproved = await IsCompanyActive();

            await Repository.UpdateAsync(existedCompany);

            return true;
        }

        public async Task<bool> IsCompanyActive()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(
                predicate: x => x.AppUserId == userId && !x.IsDeleted,
                include: x => x
                .Include(x => x.CompanySocials).Include(x => x.CompanyTranslations)
                .Include(x => x.Addresses));

            if (existedCompany == null)
                return false;

            if (existedCompany.CompanyTranslations.Any() && existedCompany.Addresses.Any()
                && existedCompany.CompanyEmail != null)
                return true;
            else
                return false;
        }

        public async Task<PagedResultModel<CompanyViewModel>> GetPagedCompaniesAsync(CompanyFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();
            var predicate = BuildPredicate(filter, language.Id);
            var orderBy = BuildOrderBy(filter, language.Id);
            var pagedCompanies = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: orderBy,
                include: x => x
                .Include(x=>x.Jobs).ThenInclude(x=>x.JobTranslations)
                .Include(x => x.CompanyTranslations)
                .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x=>x.Addresses).ThenInclude(x=>x.City).ThenInclude(x=>x.CityTranslations.Where(t=>t.LanguageId==language.Id))
                .Include(x=>x.Addresses).ThenInclude(x=>x.City).ThenInclude(x=>x.Country).ThenInclude(x=>x.Translations.Where(t=>t.LanguageId==language.Id)),
                index: filter.Index,
                size: filter.Size);
            
            var companyModels = new List<CompanyViewModel>();
            foreach (var item in pagedCompanies.Items)
            {
                var model = Mapper.Map<CompanyViewModel>(item);

                foreach(var translation in item.CompanyTranslations)
                {
                    foreach(var lang in languages)
                    {
                        if(translation.LanguageId==lang.Id)
                            model.ReadyLanguages.Add(lang);
                    }
                }

                foreach(var lang in languages)
                {
                    if(!model.ReadyLanguages.Contains(lang))
                        model.EmptyLanguages.Add(lang);
                }

                model.ActiveJobCount = 0;
                foreach(var job in item.Jobs)
                {                   
                    if (job.JobTranslations.FirstOrDefault(x => x.LanguageId == language.Id) != null && job.IsActive
                       && !job.IsDeleted && job.ExpirationDate > DateTime.UtcNow)
                        model.ActiveJobCount++;
                }

                foreach(var readyLang in model.ReadyLanguages)
                {
                    if(readyLang.Id==language.Id)
                        companyModels.Add(model);
                }
            }

            var pagedCompanyModels = new PagedResultModel<CompanyViewModel>
            {
                Items = companyModels,
                Index = pagedCompanies.Index,
                Size = pagedCompanies.Size,
                Count = companyModels.Count(),
                Pages = pagedCompanies.Pages
            };

            return pagedCompanyModels;
                
        }

        private Expression<Func<Company, bool>> BuildPredicate(CompanyFilterViewModel filter, int languageId)
        {
            Expression<Func<Company, bool>> predicate = x => !x.IsDeleted && x.IsAccountApproved &&
            (string.IsNullOrEmpty(filter.SearchTerm) ||
            x.CompanyTranslations.Any(t => t.LanguageId == languageId && (t.Name.Contains(filter.SearchTerm) ||
            t.Description.Contains(filter.SearchTerm)))) &&
            ((filter.TypeIds == null || filter.TypeIds.Count == 0 ||
            filter.TypeIds.Contains(x.CompanyTypeId)) &&
            (filter.CityIds == null || filter.CityIds.Count == 0 ||
            x.Addresses.Any(a => filter.CityIds.Contains(a.CityId))));

            return predicate;
        }

        private Func<IQueryable<Company>, IOrderedQueryable<Company>> BuildOrderBy(CompanyFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "title";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];      
                sortOrder = parts[1];   
            }

            return queryable =>
            {
                IOrderedQueryable<Company> ordered = sortBy switch
                {
                    "title" or "name" => sortOrder == "asc"
                        ? queryable.OrderBy(c => c.CompanyTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Name)
                            .FirstOrDefault())
                        : queryable.OrderByDescending(c => c.CompanyTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Name)
                            .FirstOrDefault()),

                    "lastpostedjob" => sortOrder == "asc"
                        ? queryable.OrderBy(c => c.LastPostedJob)
                        : queryable.OrderByDescending(c => c.LastPostedJob),

                    _ => sortOrder == "asc"
                        ? queryable.OrderBy(c => c.CreatedAt)
                        : queryable.OrderByDescending(c => c.CreatedAt)
                };
                return ordered;
            };
        }
    }

}
