using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CandidateManager : CrudManager<Candidate, CandidateViewModel, CandidateCreateViewModel, CandidateUpdateViewModel>
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
        private readonly IEducationService _educationService;
        private readonly IEducationTranslationService _educationTranslationService;
        private readonly IExperienceService _experienceService;
        private readonly IExperienceTranslationService _experienceTranslationService;
        private readonly ILanguageService _languageService;
        private readonly IAddressService _addressService;
        private readonly IAddressTranslationService _addressTranslationService;
        private readonly ICookieService _cookieService;

        public CandidateManager(IRepositoryAsync<Candidate> repository, IMapper mapper, IPersonalInfoService personalInfoService, IEnumService enumService, IHttpContextAccessor httpContextAccessor, IResumeService resumeService, IPersonalInfoTranslationService personalInfoTranslationService, IResumeTranslationService resumeTranslationService, FileService fileService, ICloudinaryService cloudinaryService, ICityService cityService, IEducationService educationService, IEducationTranslationService educationTranslationService, IExperienceService experienceService, IExperienceTranslationService experienceTranslationService, ILanguageService languageService, IAddressService addressService, IAddressTranslationService addressTranslationService, ICookieService cookieService) : base(repository, mapper)
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
            _educationService = educationService;
            _educationTranslationService = educationTranslationService;
            _experienceService = experienceService;
            _experienceTranslationService = experienceTranslationService;
            _languageService = languageService;
            _addressService = addressService;
            _addressTranslationService = addressTranslationService;
            _cookieService = cookieService;
        }

        public async Task<Candidate> GetCandidate(int languageId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await Repository.GetAsync(predicate: x => x.AppUserId == userId,
                include: x => x
                .Include(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations.Where(t=>t.LanguageId==languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Translations.Where(t=>t.LanguageId==languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations.Where(t=>t.LanguageId==languageId))
                .Include(x => x.Resume).ThenInclude(x => x.Experiences).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId)));

            return candidate!;
        }

        public async Task<PersonalInfoUpdateViewModel> GetPersonalInfoUpdateViewModel(int languageId)
        {
            var candidate = await GetCandidate(languageId);
            var genderList = _enumService.GetGenderListItems();
            if (candidate == null || candidate.Resume == null || candidate.Resume.PersonalInfo == null)
                return null!;

            var dashboardModel = await GetDashboardViewModel();

            var personalInfo = candidate.Resume.PersonalInfo;
            var model = new PersonalInfoUpdateViewModel
            {
                Id = personalInfo.Id,
                GenderItems=genderList,
                GenderId=(int)personalInfo.Gender,
                WorkEmail = personalInfo.WorkEmail,
                PhoneNumber = personalInfo.PhoneNumber,
                Gender = personalInfo.Gender.ToString(),
                BirthDate = personalInfo.BirthDate,
                ResumeId = personalInfo.ResumeId,
                ImageUrl = personalInfo.ImageUrl,
                DashboardModel=dashboardModel
            };

            return model;      
        }

        //public async Task<bool> UpdatePersonalInfo (PersonalInfoUpdateViewModel model)
        //{
        //    var candidate = await GetCandidate(model.LanguageId);
        //    if (candidate == null || candidate.Resume == null || candidate.Resume.PersonalInfo == null)
        //        return false;

        //    var dashboardModel = await GetDashboardViewModel();

        //    var personalInfo = candidate.Resume.PersonalInfo;
        //}
        public async Task<PersonalInfoCreateViewModel> GetPersonalInfoCreateViewModel(int languageId)
        {
            var genders = _enumService.GetGenderListItems();
            var dashboardModel = await GetDashboardViewModel();
            var resumeId = 0;
            var candidate = await GetCandidate(languageId);
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

        public async Task<bool> CreatePersonalInfo(PersonalInfoCreateViewModel model)
        {
            var candidate = await GetCandidate(model.LanguageId);
            if (candidate == null)
                return false;


            var resumeCreateViewModel = new ResumeCreateViewModel
            {
                CandidateId = candidate.Id,
            };

            var resume = await _resumeService.CreateAsync(resumeCreateViewModel);
            if (resume == null)
                return false;

            model.ResumeId = resume.Id;

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
            if (result == null) return false;

            return true;
        }

        public async Task<ProfileTranslationCreateViewModel> GetProfileTranslationCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate(languageId);
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
            var dashboardModel = await GetDashboardViewModel();

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
            var candidate = await GetCandidate(languageId);
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

            return true;
        }

        public async Task<ProfileCreateViewModel> GetProfileCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate(languageId);
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
            var dashboardModel = await GetDashboardViewModel();

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

        public async Task<bool> CreateProfile(int languageId, ProfileCreateViewModel model)
        {
            var candidate = await GetCandidate(languageId);
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
                Languages = model.resumeTranslationModel.Languages,
                Skills = model.resumeTranslationModel.Skills,
            };

            var resultResumeTranslation = await _resumeTranslationService.Create(resumeTranslationCreateViewModel, candidate.Resume.Id);
            if (resultResumeTranslation == null)
            {
                await _personalInfoTranslationService.DeleteAsync(resultPersonalInfo.Id);
                return false;
            }

            return true;

        }

        public async Task<EducationPageCreateViewModel> GetEducationTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate(languageId);
            if (candidate == null || candidate.Resume == null)
                return null!;

            var educations = candidate.Resume.Educations;
            var models = new List<EducationCreateViewModel>();

            foreach (var education in educations)
            {
                var model = new EducationCreateViewModel
                {
                    IdForTranslation = education.Id,
                    LanguageId = languageId,
                    StartDate = education.StartDate,
                    EndDate = education.EndDate,
                    EducationTypeId = (int)education.EducationType
                };

                models.Add(model);
            }

            var dashboardModel = await GetDashboardViewModel();
            var pageModel = new EducationPageCreateViewModel
            {
                Models = models,
                LanguageId = languageId,
                DashboardModel = dashboardModel,
            };

            return pageModel;
        }

        public async Task<ExperiencePageCreateViewModel> GetExperiencePageCreateViewModel(int languageId)
        {
            var dashboardModel = await GetDashboardViewModel();

            var model = new ExperiencePageCreateViewModel
            {
                LanguageId = languageId,
                DashboardViewModel = dashboardModel,
            };

            return model;
        }

        public async Task<ExperiencePageCreateViewModel> GetExperienceTranslationPageCreateViewModel(int languageId)
        {
            var candidate = await GetCandidate(languageId);
            if (candidate == null || candidate.Resume == null)
                return null!;

            var experiences = candidate.Resume.Experiences;
            var models = new List<ExperienceCreateViewModel>();
            var dashboardModel = await GetDashboardViewModel();

            foreach(var experience in experiences)
            {
                var model = new ExperienceCreateViewModel
                {
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate,
                    Translation = new ExperienceTranslationCreateViewModel
                    {
                        ExperienceId = experience.Id,
                    }
                };
                models.Add(model);
            }

            var experiencePageModel = new ExperiencePageCreateViewModel
            {
                Models = models,
                DashboardViewModel = dashboardModel,
                LanguageId = languageId
            };

            return experiencePageModel;
        }

        public async Task<bool> CreateExperienceTranslation(int languageId, ExperiencePageCreateViewModel model)
        {
            var candidate = await GetCandidate(languageId);
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            foreach (var experience in model.Models)
            {
                var experienceTranslationCreateModel = new ExperienceTranslationCreateViewModel
                {
                    ExperienceId = experience.Translation.ExperienceId,
                    LanguageId = languageId,
                    CompanyName = experience.Translation.CompanyName,
                    Position = experience.Translation.Position,
                    Responsibility = experience.Translation.Responsibility
                };

                await _experienceTranslationService.CreateAsync(experienceTranslationCreateModel);
            }

            return true;

        }

        public async Task<bool> CreateEducationTranslation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await GetCandidate(languageId);
            if (candidate == null || candidate.Resume == null)
            {
                return false;
            }

            foreach(var education in model.Models)
            {
                var educationTranslationCreateModel = new EducationTranslationCreateViewModel
                {
                    EducationId = education.IdForTranslation,
                    LanguageId = languageId,
                    MajorName = education.MajorName,
                    SchoolName = education.SchoolName,
                };

                await _educationTranslationService.CreateAsync(educationTranslationCreateModel);
            }

            return true;
        }

        public async Task<EducationPageCreateViewModel> GetEducationPageCreateViewModel(int languageId)
        {
            var educationTypes = _enumService.GetEducationTypeListItems();
            var dashboardModel = await GetDashboardViewModel();

            var model = new EducationPageCreateViewModel
            {
                EducationTypes = educationTypes,
                LanguageId = languageId,
                DashboardModel=dashboardModel
            };

            return model;
        }

        public async Task<bool> CreateEducation(int languageId, EducationPageCreateViewModel model)
        {
            var candidate = await GetCandidate(languageId);
            if(candidate ==null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach(var educationCreateViewModel in model.Models)
            {
                var result = await _educationService.Create(candidate.Resume.Id, educationCreateViewModel);

                if (result != null)
                {
                    resultIds.Add(result.Id);
                    var educationTranslationModel = new EducationTranslationCreateViewModel
                    {
                        EducationId = result.Id,
                        LanguageId = languageId,
                        SchoolName = educationCreateViewModel.SchoolName,
                        MajorName = educationCreateViewModel.MajorName,
                    };
                    await _educationTranslationService.CreateAsync(educationTranslationModel);
                }
                else
                {
                    foreach (var id in resultIds)
                    {
                        await _educationService.DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CreateExperience(int languageId, ExperiencePageCreateViewModel model)
        {
            var candidate = await GetCandidate(languageId);

            if(candidate ==null || candidate.Resume == null)
            {
                return false;
            }

            var resultIds = new List<int>();
            foreach (var experienceCreateViewModel in model.Models)
            {
                experienceCreateViewModel.ResumeId = candidate.Resume.Id;
                experienceCreateViewModel.Translation.LanguageId=languageId;
                var result = await _experienceService.CreateAsync(experienceCreateViewModel);
                if (result != null)
                {
                    resultIds.Add(result.Id);
                    experienceCreateViewModel.Translation.ExperienceId = result.Id;
                    await _experienceTranslationService.CreateAsync(experienceCreateViewModel.Translation);
                }
                else
                {
                    foreach (var id in resultIds)
                    {
                        await _experienceService.DeleteAsync(id);
                    }
                    return false;
                }
            }

            return true;
        }

        public async Task<CandidateDashboardViewModel> GetDashboardViewModel()
        {
            var languageSelected = await _cookieService.GetLanguageAsync();
            var candidate = await GetCandidate(languageSelected.Id);
            var languages = await _languageService.GetAllAsync();

            var empty = new List<LanguageViewModel>();
            var ready = new List<LanguageViewModel>();

            if (candidate.Resume == null)
            {
                foreach (var language in languages)
                {
                    empty.Add(language);
                }
            }

            else
            {
                foreach (var translation in candidate.Resume.Translations)
                {
                    ready.Add(languages.FirstOrDefault(x => x.Id == translation.LanguageId)!);
                }
                foreach (var language in languages)
                {
                    if (!ready.Contains(language))
                    {
                        empty.Add(language);
                    }
                }
            }

            var model = new CandidateDashboardViewModel
            {
                EmptyLanguages = empty,
                ReadyLanguages = ready
            };

            return model;
        }
    }
}
