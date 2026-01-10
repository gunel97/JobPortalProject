using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ProfileViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;

namespace JobPortalProject.BL.Services.Implementations
{
    public class PersonalInfoManager : CrudManager<PersonalInfo, PersonalInfoViewModel, PersonalInfoCreateViewModel, PersonalInfoUpdateViewModel>
        , IPersonalInfoService
    {
        private readonly ICandidateService _candidateService;
        private readonly IEnumService _enumService;
        private readonly FileService _fileService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IPersonalInfoTranslationService _personalInfoTranslationService;
        private readonly IResumeTranslationService _resumeTranslationService;
        private readonly IAddressService _addressService;
        private readonly IAddressTranslationService _addressTranslationService;
        private readonly ICityService _cityService;
        private readonly ICookieService _cookieService;

        public PersonalInfoManager(IRepositoryAsync<PersonalInfo> repository, IMapper mapper, ICandidateService candidateService, IEnumService enumService, FileService fileService, ICloudinaryService cloudinaryService, IPersonalInfoTranslationService personalInfoTranslationService, IResumeTranslationService resumeTranslationService, IAddressService addressService, IAddressTranslationService addressTranslationService, ICityService cityService, ICookieService cookieService) : base(repository, mapper)
        {
            _candidateService = candidateService;
            _enumService = enumService;
            _fileService = fileService;
            _cloudinaryService = cloudinaryService;
            _personalInfoTranslationService = personalInfoTranslationService;
            _resumeTranslationService = resumeTranslationService;
            _addressService = addressService;
            _addressTranslationService = addressTranslationService;
            _cityService = cityService;
            _cookieService = cookieService;
        }

        public async Task<PersonalInfoViewModel> GetPersonalInfoViewModel(int resumeId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var personalInfo = await Repository.GetAsync(predicate: x => x.ResumeId == resumeId,
                include: x => x.Include(x => x.Translations.Where(t => t.LanguageId==language.Id)).Include(x=>x.Address!));

            if (personalInfo == null || personalInfo.Translations.FirstOrDefault() == null || personalInfo.Address==null)
                return null!;
            var address = await _addressService.GetAsync(predicate: x => x.Id == personalInfo.AddressId,
                include: x => x.Include(x => x.AddressTranslations)
                .Include(x => x.City).ThenInclude(x => x.CityTranslations.Where(t => t.LanguageId==language.Id))
                .Include(x=>x.City).ThenInclude(x=>x.Country).ThenInclude(x=>x.Translations.Where(t=>t.LanguageId==language.Id)));

            var model = new PersonalInfoViewModel
            {
                Id = personalInfo.Id,
                ResumeId = personalInfo.ResumeId,
                FirstName = personalInfo.Translations.FirstOrDefault().FirstName,
                LastName = personalInfo.Translations.FirstOrDefault().LastName,
                ImageUrl = personalInfo.ImageUrl,
                PhoneNumber = personalInfo.PhoneNumber,
                WorkEmail = personalInfo.WorkEmail,
                Gender = personalInfo.Gender.ToString(),
                BirthDate = personalInfo.BirthDate,
                Address = address,
            };

            return model;
        }

        public async Task<bool> AddAddressToPersonalInfo(int personalInfoId, Address address)
        {
            var personalInfo = await Repository.GetByIdAsync(personalInfoId);
            if (personalInfo == null)
                return false;
            personalInfo.Address = address;
            var result = await Repository.UpdateAsync(personalInfo);
            if (result == null) return false;
            return true;
        }

        public async Task<PersonalInfoUpdateViewModel> GetPersonalInfoUpdateViewModel(int languageId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var candidate = await _candidateService.GetCandidateWithTranslation(languageId);
            var genderList = _enumService.GetGenderListItems();
            if (candidate == null || candidate.Resume == null ||
                candidate.Resume.PersonalInfo == null || candidate.Resume.PersonalInfo.Address == null)
                return null!;

            var dashboardModel = await _candidateService.GetDashboardViewModel();
            var cities = await _cityService.GetCitySelectListItemsWithCountry(language.Id);

            var personalInfo = candidate.Resume.PersonalInfo;
            var model = new PersonalInfoUpdateViewModel
            {
                Id = personalInfo.Id,
                CitiesList = cities,
                CityId = candidate.Resume.PersonalInfo.Address.CityId,
                GenderItems = genderList,
                GenderId = (int)personalInfo.Gender,
                WorkEmail = personalInfo.WorkEmail,
                PhoneNumber = personalInfo.PhoneNumber,
                Gender = personalInfo.Gender.ToString(),
                BirthDate = personalInfo.BirthDate,
                ResumeId = personalInfo.ResumeId,
                ImageUrl = personalInfo.ImageUrl,
                DashboardModel = dashboardModel,
                LanguageId = languageId
            };

            return model;
        }

        public async Task<bool> UpdatePersonalInfo(PersonalInfoUpdateViewModel model)
        {
            var candidate = await _candidateService.GetCandidateWithTranslation(model.LanguageId);
            var dashboardModel = await _candidateService.GetDashboardViewModel();
            if (candidate == null || candidate.Resume == null
                || candidate.Resume.PersonalInfo == null || candidate.Resume.PersonalInfo.Address == null)
                return false;
            var personalInfo = candidate.Resume.PersonalInfo;

            if (model.ImageFile != null)
            {
                if (!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));
                var imageResult = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.CandidateProfilImagePath);

                if (imageResult.Success)
                {
                    if (personalInfo.ImagePublicId != null)
                        await _cloudinaryService.DeleteImageAsync(personalInfo.ImagePublicId);
                    personalInfo.ImagePublicId = imageResult.PublicId;
                    personalInfo.ImageUrl = imageResult.Url;
                }
            }

            personalInfo.BirthDate = model.BirthDate;
            personalInfo.Gender = (Gender)model.GenderId;
            personalInfo.WorkEmail = model.WorkEmail;
            personalInfo.PhoneNumber = model.PhoneNumber;
            personalInfo.Address.CityId = model.CityId;

            var result = await Repository.UpdateAsync(personalInfo);
            if (result == null)
                return false;

            return true;
        }

        public async Task<PersonalInfoCreateViewModel> GetPersonalInfoCreateViewModel(int languageId)
        {
            var genders = _enumService.GetGenderListItems();
            var dashboardModel = await _candidateService.GetDashboardViewModel();
            var resumeId = 0;
            var candidate = await _candidateService.GetCandidate();
            if (candidate.Resume == null)
                resumeId = 0;
            else
                resumeId = candidate.Resume.Id;

            var model = new PersonalInfoCreateViewModel
            {
                LanguageId = languageId,
                GenderItems = genders,
                DashboardModel = dashboardModel,
                ResumeId = resumeId
            };

            return model;
        }

        public async Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model, int resumeId)
        {
            model.ResumeId = resumeId;

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

            var result = await CreateAsync(model);
            if (result == null) return false;

            return true;
        }

        public async Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null) return false;

            if (candidate.Resume == null || candidate.Resume.PersonalInfo == null)
                return false;

            var address = new Address
            {
                IsMainAddress = true,
                PersonalInfo = candidate.Resume.PersonalInfo,
                CityId = model.CityId
            };

            var addressTranslation = new AddressTranslation
            {
                Street = model.Street!,
                LanguageId = languageId
            };

            address.AddressTranslations.Add(addressTranslation);

            var addressResult = await AddAddressToPersonalInfo(candidate.Resume.PersonalInfo.Id, address);
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
                Languages = model.resumeTranslationModel.Languages,
                Skills = model.resumeTranslationModel.Skills,
            };

            var resultResumeTranslation = await _resumeTranslationService.Create(resumeTranslationCreateViewModel, candidate.Resume.Id);
            if (resultResumeTranslation == null)
            {
                await _personalInfoTranslationService.DeleteAsync(resultPersonalInfo.Id);
                return false;
            }
            var result = await _resumeTranslationService.Complete(resultResumeTranslation.ResumeId, languageId);
            return true;

        }

        public async Task<ProfileTranslationCreateViewModel> GetProfileTranslationCreateViewModel(int languageId)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return null!;

            var resume = new ResumeTranslationCreateViewModel
            {
                ResumeId = candidate.Resume.Id,
                LanguageId = languageId
            };

            if (candidate.Resume.PersonalInfo == null)
                return null!;

            var personalInfo = new PersonalInfoTranslationCreateViewModel
            {
                PersonalInfoId = candidate.Resume.PersonalInfo.Id,
                LanguageId = languageId
            };

            var address = await _addressService.GetAsync(predicate: x => x.Id == candidate.Resume.PersonalInfo.AddressId,
                include: x => x.Include(x => x.City).ThenInclude(x => x.CityTranslations)
                .Include(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations));

            var city = address.CityName + ", " + address.CountryName;
            var dashboardModel = await _candidateService.GetDashboardViewModel();

            var profileTranslationCreateViewModel = new ProfileTranslationCreateViewModel
            {
                personalInfoTranslationModel = personalInfo,
                resumeTranslationModel = resume,
                LanguageId = languageId,
                DashboardModel = dashboardModel,
                City = city
            };

            return profileTranslationCreateViewModel;
        }

        public async Task<bool> CreateProfileTranslation(int languageId, ProfileTranslationCreateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null) return false;

            if (candidate.Resume == null || candidate.Resume.PersonalInfo == null)
                return false;

            var addressTranslation = new AddressTranslationCreateViewModel
            {
                AddressId = candidate.Resume.PersonalInfo.AddressId!.Value,
                LanguageId = languageId,
                Street = model.Street
            };
            var resultAddress = await _addressTranslationService.CreateAsync(addressTranslation);
            if (resultAddress == null) return false;

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
                Languages = model.resumeTranslationModel.Languages,
                Skills = model.resumeTranslationModel.Skills,
                
            };

            var resultResumeTranslation = await _resumeTranslationService.Create(resumeTranslationCreateViewModel, candidate.Resume.Id);
            if (resultResumeTranslation == null)
            {
                await _addressTranslationService.DeleteAsync(resultAddress.Id);
                await _personalInfoTranslationService.DeleteAsync(resultPersonalInfo.Id);
                return false;
            }

            await _resumeTranslationService.Complete( candidate.Resume.Id,languageId);
            return true;
        }

        public async Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId)
        {
            var candidate = await _candidateService.GetCandidate();
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
            var dashboardModel = await _candidateService.GetDashboardViewModel();

            var CreateProfileViewModel = new ProfileCreateViewModel
            {
                personalInfoTranslationModel = personalInfo,
                resumeTranslationModel = resume,
                LanguageId = languageId,
                CitiesList = citiesList,
                DashboardModel = dashboardModel,
            };

            return CreateProfileViewModel;
        }

        public async Task<bool> UpdateProfileTranslation(ProfileTranslationUpdateViewModel model)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null
                || candidate.Resume.PersonalInfo == null || candidate.Resume.PersonalInfo.Address == null)
                return false;

            var resumeTranslation = candidate.Resume.Translations.FirstOrDefault(x => x.LanguageId == model.LanguageId);
            var personalInfoTranslation = candidate.Resume.PersonalInfo.Translations.FirstOrDefault(x => x.LanguageId == model.LanguageId);
            var addressTranslation = candidate.Resume.PersonalInfo.Address.AddressTranslations.FirstOrDefault(x => x.LanguageId == model.LanguageId)!;

            if (resumeTranslation == null || personalInfoTranslation == null)
                return false;

            model.ResumeTranslation.ResumeId = candidate.Resume.Id;
            model.PersonalInfoTranslation.PersonalInfoId = candidate.Resume.PersonalInfo.Id;

            var addressTranslationUpdateModel = new AddressTranslationUpdateViewModel
            {
                Id = addressTranslation.Id,
                AddressId = addressTranslation.AddressId,
                Street = model.Street,
                LanguageId = model.LanguageId
            };
            var addressResult = await _addressTranslationService.UpdateAsync(addressTranslation.Id, addressTranslationUpdateModel);

            var resumeResult = await _resumeTranslationService.Update(model.ResumeTranslation);

            var personalInfoResult = await _personalInfoTranslationService.UpdateAsync(model.PersonalInfoTranslation.Id, model.PersonalInfoTranslation);

            return true;
        }

        public async Task<ProfileUpdateViewModel> GetProfileUpdateViewModel()
        {
            var candidate = await _candidateService.GetCandidate();

            if (candidate == null || candidate.Resume==null || 
                candidate.Resume.PersonalInfo==null || candidate.Resume.PersonalInfo.Address==null)
                return null!;

            var addressTranslations = candidate.Resume.PersonalInfo.Address.AddressTranslations;
            var dashboard = await _candidateService.GetDashboardViewModel();

            var model = new ProfileUpdateViewModel
            {
                ProfileTranslations = addressTranslations.Select(x => new ProfileTranslationUpdateViewModel
                {
                    Icon = dashboard.ReadyLanguages.FirstOrDefault(t=>t.Id==x.LanguageId)!.IconUrl,
                    LanguageId = x.LanguageId,
                    Street = x.Street,
                    ResumeTranslation= new ResumeTranslationUpdateViewModel
                    {
                        Id= GetResumeTranslation(x.LanguageId, candidate).Id,
                        LanguageId=x.LanguageId,
                        About=GetResumeTranslation(x.LanguageId, candidate).About,
                        Skills=string.Join(", ",GetResumeTranslation(x.LanguageId, candidate).Skills),
                        Languages = string.Join(", ", GetResumeTranslation(x.LanguageId, candidate).Languages),
                    },
                    PersonalInfoTranslation= new PersonalInfoTranslationUpdateViewModel
                    {
                        Id = GetPersonalInfo(x.LanguageId, candidate).Id,
                        LanguageId=x.LanguageId,
                        Firstname=GetPersonalInfo(x.LanguageId, candidate).FirstName,
                        Lastname=GetPersonalInfo(x.LanguageId, candidate).LastName,
                    }
                }).ToList(),
                DashboardModel = dashboard
            };

            return model;
        }
        public ResumeTranslation GetResumeTranslation (int languageId, Candidate candidate)
        {
            return candidate.Resume!.Translations.FirstOrDefault(x => x.LanguageId == languageId)!;
        }

        public PersonalInfoTranslation GetPersonalInfo(int languageId, Candidate candidate)
        {
            return candidate.Resume!.PersonalInfo!.Translations.FirstOrDefault(x=>x.LanguageId== languageId)!;
        }
    }
}
