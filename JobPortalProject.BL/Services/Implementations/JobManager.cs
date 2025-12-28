using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Internal;

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
        private readonly IJobTranslationService _jobTranslationService;
        private readonly IJobResponsibilityService _jobResponsibilityService;
        private readonly IJobExtraBenefitService _jobBenefitService;
        private readonly IEnumService _enumService;
        public JobManager(IRepositoryAsync<Job> repository, IMapper mapper, ICookieService cookieService, IJobCategoryService jobCategoryService, IAddressService addressService, ILanguageService languageService, StringLocalizerManager localizer, IJobTranslationService jobTranslationService, IJobResponsibilityService jobResponsibilityService, IJobExtraBenefitService jobBenefitService, IEnumService enumService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _jobCategoryService = jobCategoryService;
            _addressService = addressService;
            _languageService = languageService;
            _localizer = localizer;
            _jobTranslationService = jobTranslationService;
            _jobResponsibilityService = jobResponsibilityService;
            _jobBenefitService = jobBenefitService;
            _enumService = enumService;
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
                JobTypeListItems = _enumService.GetJobTypeListItems(),
                GenderListItems = _enumService.GetGenderListItems(),
                SalaryTypeListItems = _enumService.GetSalaryTypeListItems(),
                RequiredEducationTypeListItems = _enumService.GetEducationTypeListItems(),
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
            }
            return true;
        }

        public override async Task<bool> UpdateAsync(int id, JobUpdateViewModel model)
        {
            model.JobType= (JobType)model.JobTypeId;
            model.Gender = (Gender)model.GenderId;
            model.RequiredMinEducationType = (EducationType)model.RequiredEducationTypeId;
            model.SalaryType = (SalaryTypeDuration)model.SalaryTypeId;

            return await base.UpdateAsync(id, model);
        }

        public async Task<List<JobViewModel>> GetAllJobsOfCompanyAsync(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();

            var jobs = await  Repository.GetAllAsync(predicate: x => x.CompanyId == companyId && !x.IsDeleted,
                include: x => x.Include(t => t.JobTranslations.Where(x => x.LanguageId == language.Id)));
            var jobViewModels = jobs.Select(x => MapToJobViewModel(x, language.Id));

            return jobViewModels.ToList();
        }

        public async Task<IEnumerable<JobViewModel>> GetAllJobsAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;

            var jobs = await Repository.GetAllAsync(
                predicate: x=>!x.IsDeleted,
                include: x => x
               .Include(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
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

            var job = await Repository.GetAsync(predicate: x=>!x.IsDeleted,
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
            var jobs = await Repository.GetAllAsync( predicate: x=>!x.IsDeleted,
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

        public async Task<PagedResultModel<JobViewModel>> GetPagedJobsAsync(JobFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            Expression<Func<Job, bool>> predicate = BuildPredicate(filter, language.Id);
            var pagedJobs = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: x=>x.OrderByDescending(j=>j.CreatedAt),
                include: x => x
                .Include(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == language.Id))
                , index:filter.Index, size:filter.Size);

            var jobViewModels = new List<JobViewModel>();
            foreach(var item in pagedJobs.Items)
            {
                var model = MapToJobViewModel(item, language.Id);
                jobViewModels.Add(model);
            }

            var pagedJobModels = new PagedResultModel<JobViewModel>
            {
                Items = jobViewModels,
                Index = pagedJobs.Index,
                Size = pagedJobs.Size,
                Count = pagedJobs.Count,
                Pages = pagedJobs.Pages,
            };

            return pagedJobModels;
        }

        public async Task<Dictionary<int, int>> GetJobCountGender()
        {
            var jobs = await Repository.GetAllAsync(predicate: x=>!x.IsDeleted  && x.IsActive);
            var result = jobs.GroupBy(x => (int)x.Gender).ToDictionary(g => g.Key, g => g.Count());

            return result;
        }

        public async Task<Dictionary<int, int>> GetJobCountJobType()
        {
            var jobs = await Repository.GetAllAsync(predicate: x=>!x.IsDeleted  && x.IsActive);
            var result = jobs.GroupBy(x => (int)x.JobType).ToDictionary(g => g.Key, g => g.Count());

            return result;
        }

        private JobViewModel MapToJobViewModel (Job jobEntity, int languageId)
        {
            var jobViewModel =  new JobViewModel
            {
                Id = jobEntity.Id,
                Title = jobEntity.JobTranslations.FirstOrDefault()?.Title,
                Description = jobEntity.JobTranslations.FirstOrDefault()?.Description,
                RequiredExperience=jobEntity.JobTranslations.FirstOrDefault()?.RequiredExperience,
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
                ExtraBenefits = jobEntity.ExtraBenefits.SelectMany(r => r.JobExtraBenefitTranslations
                                                  .Where(t => t.LanguageId == languageId)
                                                  .Select(t => t.Value!)).Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                CompanyImages = jobEntity.Company!.CompanyImages.Select(x=>x.ImageUrl).ToList()
            };

            return jobViewModel;
        }

        public async Task<JobUpdateViewModel> GetUpdateViewModel(int jobId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var job = await Repository.GetAsync(predicate: x => x.Id == jobId,
                include: x => x.Include(x => x.JobTranslations).Include(x => x.Address)
                .Include(x => x.ExtraBenefits).ThenInclude(x => x.JobExtraBenefitTranslations)
                .Include(x => x.Responsibilities).ThenInclude(x => x.JobResponsibilityTranslations));

            if (job == null)
                return null!;

            var model = Mapper.Map<JobUpdateViewModel>(job);
            var addressesList = await _addressService.GetAddressSelectListItems(job.CompanyId, language.Id);
            var jobCategoriesList = await _jobCategoryService.GetJobCategorySelectListItems(language.Id);
            var languages = await _languageService.GetAllAsync();

            model.RequiredEducationTypeListItems = _enumService.GetEducationTypeListItems();
            model.GenderListItems = _enumService.GetGenderListItems();
            model.SalaryTypeListItems=_enumService.GetSalaryTypeListItems();
            model.JobTypeListItems= _enumService.GetJobTypeListItems();
            model.AddressesList = addressesList;
            model.JobCategoriesList= jobCategoriesList;
            foreach(var translation in model.JobTranslations)
            {
                var languageT = languages.FirstOrDefault(x=>x.Id==translation.LanguageId);
                translation.LanguageIcon = languageT.IconUrl;
            }

            return model;
        }

        public async Task<bool> SoftDeleteJob(int id)
        {
            var job = await Repository.GetByIdAsync(id);
            if (job == null)
                return false;

            job.IsDeleted = true;
            await Repository.UpdateAsync(job);
            return true;
        }

        public async Task<bool> DeactivateJob(int id)
        {
            var job = await Repository.GetByIdAsync(id);
            if (job == null)
                return false;

            if (job.IsActive)
            {
                job.IsActive = false;
                await Repository.UpdateAsync(job);
            }
            else
            {
                job.IsActive = true;
                await Repository.UpdateAsync(job);
            }
                return true;
        }

        private Expression<Func<Job, bool>> BuildPredicate(JobFilterViewModel filter, int languageId)
        {
            Expression<Func<Job, bool>> predicate = x => !x.IsDeleted && x.IsActive &&
            (string.IsNullOrEmpty(filter.SearchTerm) ||
            x.JobTranslations.Any(t => t.LanguageId == languageId && (t.Title.Contains(filter.SearchTerm) ||
            t.Description.Contains(filter.SearchTerm))) ||
            x.Company.CompanyTranslations.Any(t => t.LanguageId == languageId && t.Name.Contains(filter.SearchTerm))) &&
            ((!filter.MinSalary.HasValue || x.MaxSalary >= filter.MinSalary.Value) &&
            (!filter.MaxSalary.HasValue || x.MinSalary <= filter.MaxSalary.Value) &&
            (filter.CategoryIds == null || filter.CategoryIds.Count == 0 ||
            filter.CategoryIds.Contains(x.JobCategoryId)) &&
            (filter.JobTypeIds == null || filter.JobTypeIds.Count == 0 ||
            filter.JobTypeIds.Contains((int)x.JobType)) && 
            (filter.GenderIds==null || filter.GenderIds.Count==0 ||
            filter.GenderIds.Contains((int)x.Gender)));

            return predicate;
        }

        }
}
