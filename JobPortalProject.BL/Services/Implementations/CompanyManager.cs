using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public CompanyManager(IRepositoryAsync<Company> repository, IMapper mapper, ICompanyTypeService companyTypeService, ICookieService cookieService, IHttpContextAccessor httpContextAccessor, ICompanySocialService companySocialService, ICloudinaryService cloudinaryService, FileService fileService, ICompanyTranslationService translationService, IWorkingFieldService workingFieldService, IWorkingFieldTranslationService workingFieldTranslationService, IAddressService addressService) : base(repository, mapper)
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
        }

        public async Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var companyCreateViewModel = new CompanyCreateViewModel();

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems(language.Id);

            companyCreateViewModel.CompanyTypeList = companyTypeSelectListItems;

            return companyCreateViewModel;
        }

        public async Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync(int selectedLanguageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return null!;

            var company = await Repository.GetAsync(
                                           predicate: x => !x.IsDeleted && x.Id == existedCompany.Id,
                                           include: x => x
                                           .Include(ct => ct.CompanyTranslations!.Where(x => x.LanguageId == selectedLanguageId))
                                           .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations.Where(x => x.LanguageId == selectedLanguageId))
                                           .Include(t => t.CompanyType!).ThenInclude(ct => ct.CompanyTypeTranslations!.Where(x => x.LanguageId == selectedLanguageId))
                                           .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations.Where(x => x.LanguageId == selectedLanguageId)));

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == existedCompany.Id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems(selectedLanguageId);
            var workingFieldUpdateViewModels = await _workingFieldService.GetUpdateViewModelAsync(existedCompany.Id, selectedLanguageId);
            var addressUpdateViewModels = await _addressService.GetAddressUpdateViewModels(existedCompany.Id, selectedLanguageId);

            var companyUpdateViewModel = new CompanyUpdateViewModel
            {
                Id = company!.Id,
                SelectedUpdateLanguageId= selectedLanguageId,
                CompanySize = company.CompanySize,
                CompanyEmail = company.CompanyEmail,
                CoverPhotoUrl = company.CoverPhotoUrl,
                LogoUrl = company.LogoUrl,
                CompanyTypeId = company.CompanyTypeId,
                CompanyTypeList = companyTypeSelectListItems,
                CompanyTranslationUpdateViewModel = new CompanyTranslationUpdateViewModel
                {
                    Id = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == selectedLanguageId)!.Id,
                    Name = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == selectedLanguageId)!.Name,
                    Description = company.CompanyTranslations.FirstOrDefault()!.Description,
                    CompanyId = company.Id,
                    LanguageId = selectedLanguageId
                },
                WorkingFieldUpdateViewModels=workingFieldUpdateViewModels,
                CompanyAddressUpdateViewModels=addressUpdateViewModels,
                CompanySocialUpdateViewModels = companySocials.Select(x => new CompanySocialUpdateViewModel
                {
                    Id = x.Id,
                    CompanyId = x.Id,
                    SocialMediaId = x.SocialMediaId,
                    AddressUrl = x.AddressUrl,
                    Title=x.SocialMedia!.Title,
                    IconUrl=x.SocialMedia.IconUrl
                }).ToList()
            };

            return companyUpdateViewModel;
        }

        public async Task<bool> IsActive(int companyId, int languageId)
        {
            var company = await Repository.GetAsync(
                predicate: x => x.Id == companyId && !x.IsDeleted,
                include: x => x.
                Include(c => c.CompanySocials).
                Include(c => c.Addresses).ThenInclude(t => t.AddressTranslations.Where(x => x.Id == languageId)).
                Include(c => c.CompanyTranslations.Where(x => x.Id == languageId)).
                Include(c => c.WorkingFields).ThenInclude(t => t.Translations.Where(x => x.Id == languageId))
                );

            if (company == null)
                return false;

            if (company.CompanyTypeId == 0 || company.CompanyEmail == null || !company.CompanySocials.Any() ||
                company.CompanyTranslations.FirstOrDefault(x => x.Id == languageId)!.Name == null ||
                company.CompanyTranslations.FirstOrDefault(x => x.Id == languageId)!.Description == null ||
                !company.WorkingFields.Any() || !company.Addresses.Any())
                return false;

            return true;
        }

        public async Task<WorkingFieldCreateViewModel> GetWorkingFieldCreateViewModel(int selectedLanguageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return null!;

            var workingFieldCreateViewModel = new WorkingFieldCreateViewModel
            {
                CompanyId = existedCompany.Id,
                WorkingFieldTranslationCreateViewModel = new WorkingFieldTranslationCreateViewModel
                {
                    LanguageId = selectedLanguageId
                }
            };

            return workingFieldCreateViewModel;
        }

        public async Task<AddWorkingFieldTranslationViewModel> GetAddTranslationViewModelAsync(int selectedLanguageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return null!;

            var selectListItems = await _workingFieldService.GetWorkingFieldSelectListItemsAsync(existedCompany.Id, selectedLanguageId);
            var model = new AddWorkingFieldTranslationViewModel
            {
                SelectedLanguageId = selectedLanguageId,
                WorkingFields = selectListItems
            };

            return model;
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
                var workingFieldTranslationCreateModel = new WorkingFieldTranslationCreateViewModel
                {
                    WorkingFieldId = workingField.Id,
                    LanguageId = model.SelectedUpdateLanguageId,
                    Name = model.WorkingFieldTranslationCreateViewModel.Name,
                    Description = model.WorkingFieldTranslationCreateViewModel.Description,
                };

                var workingFieldTranslation = await _workingFieldTranslationService.CreateAsync(workingFieldTranslationCreateModel);
                if (workingFieldTranslation == null)
                {
                    await _workingFieldService.DeleteAsync(workingField.Id);
                    return false;
                }
            }

            return true;
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
                var companySocial = await _companySocialService.GetAsync(predicate:x=>x.Id==companySocialModel.Id);
                companySocialModel.CompanyId = existedCompany.Id;
                companySocialModel.SocialMediaId = companySocial.SocialMediaId;
                await _companySocialService.UpdateAsync(companySocialModel.Id, companySocialModel);
            }

            var existedCompanyTranslation = await _companyTranslationService.GetAsync(
                predicate: x => x.CompanyId == existedCompany.Id && x.LanguageId == selectedLanguageId);
            model.CompanyTranslationUpdateViewModel!.CompanyId = existedCompany.Id;
            model.CompanyTranslationUpdateViewModel!.LanguageId = selectedLanguageId;
            await _companyTranslationService.UpdateAsync(existedCompanyTranslation.Id, model.CompanyTranslationUpdateViewModel);

            foreach(var addressModel in model.CompanyAddressUpdateViewModels)
            {
                await _addressService.UpdateAddressAsync(selectedLanguageId, addressModel.Id, addressModel);
            }

            foreach(var workingFieldModel in model.WorkingFieldUpdateViewModels)
            {
                var workingField = await _workingFieldService.GetAsync(predicate: x=>x.Id== workingFieldModel.Id);
                workingFieldModel.IconUrl = workingField.IconUrl;
                workingFieldModel.IconPublicId = workingField.IconPublicId;
                workingFieldModel.CompanyId = existedCompany.Id;

                if (workingFieldModel.IconFile != null) {
                    if(!_fileService.IsImageFile(workingFieldModel.IconFile))
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

                await _workingFieldService.UpdateAsync(workingFieldModel.Id, workingFieldModel);


                var workingFieldTranslationModel = workingFieldModel.WorkingFieldTranslationUpdateViewModel;
                workingFieldTranslationModel.WorkingFieldId = workingFieldModel.Id;
                workingFieldTranslationModel.LanguageId = selectedLanguageId;
                await _workingFieldTranslationService.UpdateAsync(workingFieldTranslationModel.Id, workingFieldTranslationModel);

            }

            await Repository.UpdateAsync(existedCompany);

            return true;
        }
    }


}
