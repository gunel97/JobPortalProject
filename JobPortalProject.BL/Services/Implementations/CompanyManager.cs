using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private readonly IWorkingFieldTranslationService _workingFieldTranslationService;
        private readonly IAddressService _addressService;
        private readonly IAddressTranslationService _addressTranslationService;
        private readonly ILanguageService _languageService;
        private readonly ICityService _cityService;

        public CompanyManager(IRepositoryAsync<Company> repository, IMapper mapper, ICompanyTypeService companyTypeService, ICookieService cookieService, IHttpContextAccessor httpContextAccessor, ICompanySocialService companySocialService, ICloudinaryService cloudinaryService, FileService fileService, ICompanyTranslationService translationService, IWorkingFieldService workingFieldService, IWorkingFieldTranslationService workingFieldTranslationService, IAddressService addressService, ILanguageService languageService, ICityService cityService, IAddressTranslationService addressTranslationService) : base(repository, mapper)
        {
            _companyTypeService = companyTypeService;
            _cookieService = cookieService;
            _httpContextAccessor = httpContextAccessor;
            _companySocialService = companySocialService;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
            _companyTranslationService = translationService;
            _workingFieldService = workingFieldService;
            _workingFieldTranslationService = workingFieldTranslationService;
            _addressService = addressService;
            _languageService = languageService;
            _cityService = cityService;
            _addressTranslationService = addressTranslationService;
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
        public async Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var companyCreateViewModel = new CompanyCreateViewModel();

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems(language.Id);

            companyCreateViewModel.CompanyTypeList = companyTypeSelectListItems;

            return companyCreateViewModel;
        }

        public async Task<AddressCreateViewModel> GetAddressCreateViewModel()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x.Include(t => t.CompanyTranslations));
            var languages = await _languageService.GetAllAsync();
            var selectedLanguage = await _cookieService.GetLanguageAsync();

            if (existedCompany == null)
                return null!;

            var cities = await _cityService.GetCitySelectListItemsWithCountry(selectedLanguage.Id);
            var addressCreateViewModel = new AddressCreateViewModel
            {
                CompanyId = existedCompany.Id,
                SelectedLanguageId = selectedLanguage.Id,
                CompanyTranslationsCount = existedCompany.CompanyTranslations.Count(),
                CityListItems = cities,
                AddressTranslationCreateViewModels = languages.Select(x => new AddressTranslationCreateViewModel
                {
                    LanguageId = x.Id,
                }).ToList()
            };

            return addressCreateViewModel;
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

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == existedCompany.Id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems(selectedLanguageId);
            var citiesList = await _cityService.GetCitySelectListItemsWithCountry(selectedLanguageId);

            var companyUpdateViewModel = new CompanyUpdateViewModel
            {
                Id = company!.Id,
                SelectedUpdateLanguageId = selectedLanguageId,
                CompanySize = company.CompanySize,
                CompanyEmail = company.CompanyEmail,
                CoverPhotoUrl = company.CoverPhotoUrl,
                LogoUrl = company.LogoUrl,
                CompanyTypeId = company.CompanyTypeId,
                CompanyTypeList = companyTypeSelectListItems,
                CitiesList=citiesList,
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

            foreach (var translation in companyUpdateViewModel.CompanyTranslations)
            {
                translation.LanguageIcon = languages.FirstOrDefault(x => x.Id == translation.LanguageId)!.IconUrl;
            }

            return companyUpdateViewModel;
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

        public async Task<WorkingFieldCreateViewModel> GetWorkingFieldCreateViewModel()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x.Include(t => t.CompanyTranslations));
            var languages = await _languageService.GetAllAsync();

            if (existedCompany == null)
                return null!;

            var workingFieldCreateViewModel = new WorkingFieldCreateViewModel
            {
                CompanyId = existedCompany.Id,
                CompanyTranslationsCount = existedCompany.CompanyTranslations.Count(),
                WorkingFieldTranslationCreateViewModels = languages.Select(x => new WorkingFieldTranslationCreateViewModel
                {
                    LanguageId = x.Id,
                }).ToList()
            };

            return workingFieldCreateViewModel;
        }



        public async Task<bool> CreateAddress(AddressCreateViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return false;

            model.CompanyId = existedCompany.Id;
            var createdAddress = await _addressService.CreateAsync(model);

            if (createdAddress == null)
            {
                return false;
            }
            else
            {
                foreach (var translationModel in model.AddressTranslationCreateViewModels)
                {
                    translationModel.AddressId = createdAddress.Id;
                    translationModel.Street = translationModel.Street;
                    translationModel.LanguageId = translationModel.LanguageId;

                    var result = await _addressTranslationService.CreateAsync(translationModel);

                    if (result == null)
                    {
                        await _addressService.DeleteAsync(createdAddress.Id);
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<bool> CreateWorkingField(WorkingFieldCreateViewModel model)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return false;

            model.CompanyId = existedCompany.Id;
            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var result = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.WorkingFieldImagePath);

                if (result.Success)
                {
                    model.IconUrl = result.Url;
                    model.IconPublicId = result.PublicId;
                }
            }
            var workingField = await _workingFieldService.CreateAsync(model);

            if (workingField == null)
                return false;

            else
            {
                foreach (var translationModel in model.WorkingFieldTranslationCreateViewModels)
                {
                    var workingFieldTranslationCreateModel = new WorkingFieldTranslationCreateViewModel
                    {
                        WorkingFieldId = workingField.Id,
                        LanguageId = translationModel.LanguageId,
                        Name = translationModel.Name,
                        Description = translationModel.Description,
                    };

                    var workingFieldTranslation = await _workingFieldTranslationService.CreateAsync(workingFieldTranslationCreateModel);
                    if (workingFieldTranslation == null)
                    {
                        await _workingFieldService.DeleteAsync(workingField.Id);
                        return false;
                    }
                }
            }
            return true;
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
                var workingField = await _workingFieldService.GetAsync(predicate: x => x.Id == workingFieldModel.WorkingFieldId);
                workingFieldModel.IconUrl = workingField.IconUrl;
                workingFieldModel.IconPublicId = workingField.IconPublicId;
                workingFieldModel.CompanyId = existedCompany.Id;

                if (workingFieldModel.IconFile != null)
                {
                    if (!_fileService.IsImageFile(workingFieldModel.IconFile))
                        throw new ArgumentException("The file is not a valid image.", nameof(workingFieldModel.IconFile));

                    var resultLogo = await _cloudinaryService.UploadImageAsync(workingFieldModel.IconFile, FilePathConstants.WorkingFieldImagePath);

                    if (resultLogo.Success)
                    {
                        if (workingField.IconPublicId != null)
                        {
                            var deleteResult = await _cloudinaryService.DeleteImageAsync(workingField.IconPublicId);
                        }
                        workingFieldModel.IconUrl = resultLogo.Url;
                        workingFieldModel.IconPublicId = resultLogo.PublicId;
                    }
                }

                var workingFieldTranslationModel = workingFieldModel.WorkingFieldTranslationUpdateViewModel;
                if (workingFieldTranslationModel == null)
                    return false;

                workingFieldTranslationModel.WorkingFieldId = workingFieldModel.WorkingFieldId;
                workingFieldTranslationModel.LanguageId = selectedLanguageId;
                await _workingFieldTranslationService.UpdateAsync(workingFieldTranslationModel.WorkingFieldTranslationId, workingFieldTranslationModel);


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


            await Repository.UpdateAsync(existedCompany);

            return true;
        }

        public async Task<bool> IsCompanyActive()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(
                predicate: x => x.AppUserId == userId && !x.IsDeleted, 
                include: x=>x
                .Include(x=>x.CompanySocials).Include(x=>x.CompanyTranslations)
                .Include(x=>x.Addresses));

            if (existedCompany == null)
                return false;

            if (existedCompany.CompanyTranslations.Any() && existedCompany.CompanySocials.Any()
                && existedCompany.Addresses.Any() && existedCompany.CompanyEmail!=null 
                && existedCompany.LogoUrl!=null)
                return true;
            else
                return false;
        }
    }

}
