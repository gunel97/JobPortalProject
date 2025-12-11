using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Security.Claims;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobManager : 
        CrudManager<Job, JobViewModel, JobCreateViewModel, JobUpdateViewModel>
        , IJobService
    {
        private readonly IAddressService _addressService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICookieService _cookieService;
        private readonly ILanguageService _languageService;
        private readonly StringLocalizerManager _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobTranslationService _jobTranslationService;
        private readonly IJobResponsibilityService _jobResponsibilityService;
        private readonly IJobExtraBenefitService _jobBenefitService;

        public JobManager(IRepositoryAsync<Job> repository, IMapper mapper, ICookieService cookieService, IJobCategoryService jobCategoryService, IAddressService addressService, ILanguageService languageService, StringLocalizerManager localizer, IHttpContextAccessor httpContextAccessor, IJobTranslationService jobTranslationService, IJobResponsibilityService jobResponsibilityService, IJobExtraBenefitService jobBenefitService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _jobCategoryService = jobCategoryService;
            _addressService = addressService;
            _languageService = languageService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _jobTranslationService = jobTranslationService;
            _jobResponsibilityService = jobResponsibilityService;
            _jobBenefitService = jobBenefitService;
        }

        public async Task<JobCreateViewModel> GetJobCreateViewModelAsync(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var addressesList = await _addressService.GetAddressSelectListItems(companyId, language.Id);
            var jobCategoriesList = await _jobCategoryService.GetJobCategorySelectListItems(language.Id);
            var languages = await _languageService.GetAllAsync();
            var model = new JobCreateViewModel
            {
                CompanyId = companyId,
                AddressesList = addressesList,
                JobCategoriesList = jobCategoriesList,
                JobTypeListItems = GetJobTypeListItems(),
                GenderListItems = GetGenderListItems(),
                SalaryTypeListItems = GetSalaryTypeListItems(),
                RequiredEducationTypeListItems = GetEducationTypeListItems(),
                TranslationCreateViewModels = languages.Select(x => new JobTranslationCreateViewModel
                {
                    LanguageId = x.Id,
                    LanguageIcon=x.IconUrl
                }).ToList()
            };

            return model;
        }

        public async Task<bool> CreateJob(int companyId, JobCreateViewModel model)
        {
            var job = Mapper.Map<Job>(model);

            job.AddressId = model.AddressId;
            job.CompanyId = companyId;
            job.Gender = (Gender)model.GenderId;
            job.JobType = (JobType)model.JobTypeId;
            job.SalaryTypeDuration = (SalaryTypeDuration)model.SalaryTypeId;
            job.RequiredMinEducationType = (EducationType)model.RequiredEducationTypeId;
            job.IsActive = true;

            var createdJob = await Repository.AddAsync(job);
            if (createdJob == null)
                return false;

            if (createdJob != null)
            {
                foreach (var translationModel in model.TranslationCreateViewModels)
                {
                    var translationResult = await _jobTranslationService.CreateJobTranslation(createdJob.Id, translationModel);

                    if (!translationResult)
                    {
                        await Repository.DeleteAsync(createdJob);
                        return false;
                    }
                }

                foreach(var responsibility in model.Responsibilities)
                {
                    responsibility.JobId = createdJob.Id;
                    var responsibilityResult = await _jobResponsibilityService.CreateJobResponsibilityAsync(responsibility);

                    if (!responsibilityResult)
                    {
                        await Repository.DeleteAsync(createdJob);
                        return false;
                    }
                }

                foreach (var benefit in model.Benefits)
                {
                    benefit.JobId = createdJob.Id;
                    var benefitResult = await _jobBenefitService.CreateJobBenefitAsync(benefit);

                    if (!benefitResult)
                    {
                        await Repository.DeleteAsync(createdJob);
                        return false;
                    }
                }
            }

            return true;
        }

        public List<SelectListItem> GetJobTypeListItems()
        {
            var jobTypeListItems = new List<SelectListItem>();
            var jobTypes = Enum.GetValues(typeof(JobType)).Cast<JobType>().ToList();
            jobTypes.ForEach(x => jobTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return jobTypeListItems;
        }

        public List<SelectListItem> GetSalaryTypeListItems()
        {
            var salaryTypeListItems = new List<SelectListItem>();
            var salaryTypes = Enum.GetValues(typeof(SalaryTypeDuration)).Cast<SalaryTypeDuration>().ToList();
            salaryTypes.ForEach(x => salaryTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return salaryTypeListItems;
        }

        public List<SelectListItem> GetEducationTypeListItems()
        {
            var educationTypeListItems = new List<SelectListItem>();
            var educationTypes = Enum.GetValues(typeof(EducationType)).Cast<EducationType>().ToList();
            educationTypes.ForEach(x => educationTypeListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));
            return educationTypeListItems;
        }

        public List<SelectListItem> GetGenderListItems()
        {
            var genderListItems = new List<SelectListItem>();
            var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            genders.ForEach(x => genderListItems.Add(
                new SelectListItem(_localizer.GetValue(x.ToString()), ((int)x).ToString())));

            return genderListItems;
        }

        public async Task<List<JobViewModel>> GetAllJobsOfCompanyAsync(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();

            var jobs = await  Repository.GetAllAsync(predicate: x => x.CompanyId == companyId,
                include: x => x.Include(t => t.JobTranslations.Where(x => x.LanguageId == language.Id)));
            var jobViewModels = jobs.Select(x => MapToJobViewModel(x, language.Id));

            return jobViewModels.ToList();
        }

        //public override async Task<IEnumerable<JobViewModel>> GetAllAsync(Expression<Func<Job, bool>>? predicate = null, Func<IQueryable<Job>, IOrderedQueryable<Job>>? orderBy = null, Func<IQueryable<Job>, IIncludableQueryable<Job, object>>? include = null, bool AsNoTracking = false)
        //{
        //    var language = await _cookieService.GetLanguageAsync();
        //    int languageId = language.Id;


        //    var jobs = await Repository.GetAllAsync(
        //       include: x => x
        //       .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
        //       .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
        //       .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
        //       .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
        //       .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
        //       .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
        //       .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
        //        );

        //    var jobViewModels = jobs.Select(x => MapToJobViewModel(x, languageId));

        //    return jobViewModels;
        //}
        public async Task<IEnumerable<JobViewModel>> GetAllJobsAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;


            var jobs = await Repository.GetAllAsync(
               include: x => x
               .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
               .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
                );

            var jobViewModels = jobs.Select(x => MapToJobViewModel(x, languageId));

            return jobViewModels;
        }

        public override async Task<JobViewModel?> GetByIdAsync(int id)
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;

            var job = await Repository.GetAsync(predicate: x => !x.IsDeleted && x.Id == id,
                include: x => x
               .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
               .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Address!).ThenInclude(x => x.City!).ThenInclude(x => x.CityTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Address!).ThenInclude(x => x.City!).ThenInclude(x => x.Country!).ThenInclude(x => x.Translations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Company!).ThenInclude(x => x.CompanyImages)
                );

            if (job == null)
                return null!;

            var jobViewModel = MapToJobViewModel(job, languageId);

            return jobViewModel; 
        }

        public override async Task<JobViewModel> GetAsync(Expression<Func<Job, bool>> predicate, Func<IQueryable<Job>, IIncludableQueryable<Job, object>>? include = null, bool AsNoTracking = false)
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;

            var job = await Repository.GetAsync(predicate: x=>!x.IsDeleted && x.Id==14,
                include: x => x
               .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
               .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
               .Include(x=>x.Address!).ThenInclude(x=>x.City!).ThenInclude(x=>x.CityTranslations.Where(x=>x.LanguageId==languageId))
               .Include(x=>x.Address!).ThenInclude(x=>x.City!).ThenInclude(x=>x.Country!).ThenInclude(x=>x.Translations.Where(x=>x.LanguageId==languageId))
               .Include(x=>x.Company!).ThenInclude(x=>x.CompanyImages)
                );

            if (job == null)
                return null!;

            var jobViewModel = MapToJobViewModel(job, languageId);

            return jobViewModel;
        }

        public async Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId)
        {
            var jobs = await Repository.GetAllAsync(
                include: x => x
                .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
                .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
                 );

            var jobViewModels = jobs.Select(x =>MapToJobViewModel(x, languageId)).ToList();

            return jobViewModels;
        }

        private JobViewModel MapToJobViewModel (Job jobEntity, int languageId)
        {
            var jobViewModel =  new JobViewModel
            {
                Id = jobEntity.Id,
                Title = jobEntity.JobTranslations.FirstOrDefault()?.Title,
                Description = jobEntity.JobTranslations.FirstOrDefault()?.Description,
                VacancyCount = jobEntity.VacancyCount,
                MinSalary = jobEntity.MinSalary,
                MaxSalary = jobEntity.MaxSalary,
                IsActive = jobEntity.IsActive,
                ExpirationDate = jobEntity.ExpirationDate,
                CreatedAt=jobEntity.CreatedAt,
                JobCategoryId = jobEntity.JobCategoryId,
                Gender = jobEntity.Gender.ToString(),
                RequiredMinEducationType = jobEntity.RequiredMinEducationType.ToString(),
                SalaryTypeDuration= jobEntity.SalaryTypeDuration.ToString(),
                JobType=jobEntity.JobType.ToString(),
                Address = Mapper.Map<AddressViewModel>(jobEntity.Address),
                JobCategoryName = jobEntity.JobCategory?.JobCategoryTranslations.FirstOrDefault(x => x.LanguageId == languageId)?.Name,
                CompanyId = jobEntity.CompanyId,
                CompanyName = jobEntity.Company?.CompanyTranslations.FirstOrDefault(x => x.LanguageId == languageId)?.Name,
                CompanyLogoUrl = jobEntity.Company?.LogoUrl,
                Responsibilities = jobEntity.Responsibilities.SelectMany(r => r.JobResponsibilityTranslations
                                                   .Where(t => t.LanguageId == languageId)
                                                   .Select(t => t.Value!)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                MainDuties = jobEntity.MainDuties.SelectMany(r => r.JobMainDutyTranslations
                                                  .Where(t => t.LanguageId == languageId)
                                                  .Select(t => t.Value!)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                Benefits = jobEntity.ExtraBenefits.SelectMany(r => r.JobExtraBenefitTranslations
                                                  .Where(t => t.LanguageId == languageId)
                                                  .Select(t => t.Value!)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                CompanyImages = jobEntity.Company!.CompanyImages.Select(x=>x.ImageUrl).ToList()
            };

            return jobViewModel;
        }

    }
}
