using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CandidateManager:CrudManager<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
        , ICandidateService
    {
        private readonly IPersonalInfoService _personalInfoService;
        private readonly IEnumService _enumService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResumeService _resumeService;
        private readonly IPersonalInfoTranslationService _personalInfoTranslationService;
        private readonly IResumeTranslationService _resumeTranslationService;
        private readonly FileService _fileService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICityService _cityService;

        public CandidateManager(IRepositoryAsync<Candidate> repository, IMapper mapper, IPersonalInfoService personalInfoService, IEnumService enumService, IHttpContextAccessor httpContextAccessor, IResumeService resumeService, IPersonalInfoTranslationService personalInfoTranslationService, IResumeTranslationService resumeTranslationService, FileService fileService, ICloudinaryService cloudinaryService, ICityService cityService) : base(repository, mapper)
        {
            _personalInfoService = personalInfoService;
            _enumService = enumService;
            _httpContextAccessor = httpContextAccessor;
            _resumeService = resumeService;
            _personalInfoTranslationService = personalInfoTranslationService;
            _resumeTranslationService = resumeTranslationService;
            _fileService = fileService;
            _cloudinaryService = cloudinaryService;
            _cityService = cityService;
        }

        public async Task<Candidate> GetCandidate()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x.Include(x => x.Resume).ThenInclude(x=>x.PersonalInfo));

            return candidate!;
        }

        public PersonalInfoCreateViewModel GetPersonalInfoCreateViewModel()
        {
            var genders = _enumService.GetGenderListItems();
            var model = new PersonalInfoCreateViewModel
            {
                GenderItems = genders
            };

            return model;
        }

        public async Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model)
        {
            var candidate = await GetCandidate();
            if (candidate == null)
                return false;

            if (candidate.Resume != null)
            {
                var existedResumeId = candidate.Resume.Id;
                var deleted = await _resumeService.DeleteAsync(existedResumeId);
            }

            var resumeCreateViewModel = new ResumeCreateViewModel
            {
                CandidateId = candidate.Id,
            };

            var resume = await _resumeService.CreateAsync(resumeCreateViewModel);
            if(resume==null) 
                return false;

            model.ResumeId= resume.Id;

            if (model.ImageFile != null)
            {
                if (!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));

                var resultProfileImage = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.CandidateProfilImagePath);

                if (resultProfileImage.Success)
                {
                    model.ImageUrl = resultProfileImage.Url;
                    model.ImagePublicId = resultProfileImage.PublicId;
                }
            }
            else
            {
                return false;
            }

            var result = await _personalInfoService.CreateAsync(model);
            if(result==null) return false;

            return true;
        }

        public async Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate();
            if (candidate == null)
                return null!;

            if (candidate.Resume == null)
                return null!;

            var resume = new ResumeTranslationCreateViewModel
            {
                ResumeId = candidate.Resume.Id,
                LanguageId = languageId,
            };

            if (candidate.Resume.PersonalInfo == null)
                return null!;

            var personalInfo = new PersonalInfoTranslationCreateViewModel
            {
                PersonalInfoId = candidate.Resume.PersonalInfo.Id,
                LanguageId = languageId
            };

            var citiesList = await _cityService.GetCitySelectListItemsWithCountry(languageId);

            var CreateProfileViewModel = new ProfileCreateViewModel
            {
                personalInfoTranslationModel = personalInfo,
                resumeTranslationModel = resume,
                LanguageId = languageId,
                CitiesList = citiesList
            };

            return CreateProfileViewModel;
        }

        public async Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model)
        {
            var candidate = await GetCandidate();
            if (candidate == null) return false;

            if (candidate.Resume == null || candidate.Resume.PersonalInfo == null)
                return false;

            var address = new Address
            {
                IsMainAddress = true,
                PersonalInfo = candidate.Resume.PersonalInfo,
           CityId=model.CityId
            };
            var addressTranslation = new AddressTranslation
            {
                Street = model.Street!
            };

            address.AddressTranslations.Add(addressTranslation);

            var addressResult = await _personalInfoService.AddAddressToPersonalInfo(candidate.Resume.PersonalInfo.Id, address);
            if (!addressResult) return false;

            var personalInfoTranslationCreateViewModel = new PersonalInfoTranslationCreateViewModel
            {
                LanguageId = languageId,
                Firstname = model.personalInfoTranslationModel.Firstname,
                Lastname = model.personalInfoTranslationModel.Lastname,
                PersonalInfoId = candidate.Resume.PersonalInfo.Id,
            };

            var resultPersonalInfo = await _personalInfoTranslationService.CreateAsync(personalInfoTranslationCreateViewModel);

            if (resultPersonalInfo == null)
                return false;

            var resumeTranslationCreateViewModel = new ResumeTranslationCreateViewModel
            {
                LanguageId = languageId,
                ResumeId = candidate.Resume.Id,
                About = model.resumeTranslationModel.About,
                Languages=model.resumeTranslationModel.Languages,
                Skills=model.resumeTranslationModel.Skills,
            };

            var resultResumeTranslation = await _resumeTranslationService.Create(resumeTranslationCreateViewModel, candidate.Resume.Id);
            if(resultResumeTranslation== null)
            {
                await _personalInfoTranslationService.DeleteAsync(resultPersonalInfo.Id);
                return false;
            }

            return true;

        }
    }
}
