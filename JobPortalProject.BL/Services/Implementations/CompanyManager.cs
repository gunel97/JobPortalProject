using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
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
        private readonly ICompanyTranslationService _translationService;

        public CompanyManager(IRepositoryAsync<Company> repository, IMapper mapper, ICompanyTypeService companyTypeService, ICookieService cookieService, IHttpContextAccessor httpContextAccessor, ICompanySocialService companySocialService, ICloudinaryService cloudinaryService, FileService fileService, ICompanyTranslationService translationService) : base(repository, mapper)
        {
            _companyTypeService = companyTypeService;
            _cookieService = cookieService;
            _httpContextAccessor = httpContextAccessor;
            _companySocialService = companySocialService;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
            _translationService = translationService;
        }

        public async Task<CompanyCreateViewModel> GetCompanyCreateViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var companyCreateViewModel = new CompanyCreateViewModel();

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems();

            companyCreateViewModel.CompanyTypeList = companyTypeSelectListItems;

            return companyCreateViewModel;
        }

        public async Task<CompanyUpdateViewModel> GetCompanyUpdateViewModelAsync(int languageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var existedCompany = await Repository.GetAsync(predicate: x => x.AppUserId == userId);

            if (existedCompany == null)
                return null!;

            var company = await Repository.GetAsync(
                                           predicate: x => !x.IsDeleted && x.Id == existedCompany.Id,
                                           include: x => x
                                           .Include(ct => ct.CompanyTranslations!.Where(x => x.LanguageId == languageId))
                                           .Include(x => x.Addresses).ThenInclude(x => x.AddressTranslations.Where(x => x.LanguageId == languageId))
                                           .Include(t => t.CompanyType!).ThenInclude(ct => ct.CompanyTypeTranslations!.Where(x => x.LanguageId == languageId))
                                           .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations.Where(x => x.LanguageId == languageId)));

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == existedCompany.Id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var companyTypeSelectListItems = await _companyTypeService.GetCompanyTypeSelectListItems();

            var companyUpdateViewModel = new CompanyUpdateViewModel
            {
                Id = company!.Id,
                CompanySize = company.CompanySize,
                CompanyEmail = company.CompanyEmail,
                CoverPhotoUrl = company.CoverPhotoUrl,
                LogoUrl = company.LogoUrl,
                CompanyTypeId = company.CompanyTypeId,
                CompanyTypeList = companyTypeSelectListItems,
                CompanyTranslationUpdateViewModel = new CompanyTranslationUpdateViewModel
                {
                    Id = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == languageId)!.Id,
                    Name = company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == languageId)!.Name,
                    Description = company.CompanyTranslations.FirstOrDefault()!.Description,
                    CompanyId = company.Id,
                    LanguageId = languageId
                },
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

        public override async Task<bool> UpdateAsync(int id, CompanyUpdateViewModel model)
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
                    if (existedCompany.CoverPhotoPublicId != null)
                    {
                        var deleteResult = await _cloudinaryService.DeleteImageAsync(existedCompany.CoverPhotoPublicId!);
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

            var existedCompanyTranslation = await _translationService.GetAsync(
                predicate: x => x.CompanyId == existedCompany.Id && x.LanguageId == 1);
            model.CompanyTranslationUpdateViewModel!.CompanyId = existedCompany.Id;
            model.CompanyTranslationUpdateViewModel!.LanguageId = 1;
            await _translationService.UpdateAsync(existedCompanyTranslation.Id, model.CompanyTranslationUpdateViewModel);

            await Repository.UpdateAsync(existedCompany);

            return true;
        }
    }


}
