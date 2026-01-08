using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using MimeKit.Cryptography;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        private readonly IJobTranslationService _jobTranslationService;
        private readonly IEnumService _enumService;
        private readonly ICompanyTranslationService _companyTranslationService;
        private readonly IJobResponsibilityTranslationService _jobResponsibilityTranslationService;
        private readonly IJobExtraBenefitTranslationService _jobExtraBenefitTranslationService;

        public JobManager(IRepositoryAsync<Job> repository, IMapper mapper, ICookieService cookieService, IJobCategoryService jobCategoryService, IAddressService addressService, ILanguageService languageService, IJobTranslationService jobTranslationService, IEnumService enumService, ICompanyTranslationService companyTranslationService, IJobResponsibilityTranslationService jobResponsibilityTranslationService, IJobExtraBenefitTranslationService jobExtraBenefitTranslationService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _jobCategoryService = jobCategoryService;
            _addressService = addressService;
            _languageService = languageService;
            _jobTranslationService = jobTranslationService;
            _enumService = enumService;
            _companyTranslationService = companyTranslationService;
            _jobResponsibilityTranslationService = jobResponsibilityTranslationService;
            _jobExtraBenefitTranslationService = jobExtraBenefitTranslationService;
        }

        public async Task<AddTranslationToJobViewModel> GetAddTranslationToJobViewModel(int jobId, int languageId)
        {
            var job = await Repository.GetAsync(predicate: x => x.Id == jobId,
                include: x => x.Include(x => x.JobTranslations).
                Include(x => x.ExtraBenefits).ThenInclude(x => x.JobExtraBenefitTranslations)
                .Include(x => x.Responsibilities).ThenInclude(x => x.JobResponsibilityTranslations));

            if (job == null || job.JobTranslations.FirstOrDefault() == null)
                return null!;
            var readyTranslation = job.JobTranslations.FirstOrDefault();
            var readyLanguage = await _languageService.GetByIdAsync(readyTranslation!.LanguageId);
            var language = await _languageService.GetByIdAsync(languageId);

            if (readyLanguage == null)
                return null!;

            var benefitModels = new List<JobExtraBenefitTranslationCreateViewModel>();
            if (job.ExtraBenefits.Any())
            {
                foreach (var benefit in job.ExtraBenefits)
                {
                    if (benefit.JobExtraBenefitTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id) != null)
                    {
                        var model = new JobExtraBenefitTranslationCreateViewModel
                        {
                            ReadyValue = benefit.JobExtraBenefitTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id).Value,
                            LanguageId = languageId,
                            JobExtraBenefitId = benefit.Id
                        };
                        benefitModels.Add(model);
                    }
                }
                var benefitCreateModels = job.ExtraBenefits.Select(x => new JobExtraBenefitTranslationCreateViewModel
                {
                    ReadyValue = x.JobExtraBenefitTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id).Value,
                    LanguageId = languageId,
                    JobExtraBenefitId = x.Id
                }).ToList();
            }

            var responsibilityModels = new List<JobResponsibilityTranslationCreateViewModel>();
            if (job.Responsibilities.Any())
            {
                foreach (var responsibility in job.Responsibilities)
                {
                    if (responsibility.JobResponsibilityTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id) != null)
                    {
                        var model = new JobResponsibilityTranslationCreateViewModel
                        {
                            ReadyValue = responsibility.JobResponsibilityTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id).Value,
                            LanguageId = languageId,
                            JobResponsibilityId = responsibility.Id
                        };
                        responsibilityModels.Add(model);
                    }
                }
                var benefitCreateModels = job.ExtraBenefits.Select(x => new JobExtraBenefitTranslationCreateViewModel
                {
                    ReadyValue = x.JobExtraBenefitTranslations.FirstOrDefault(x => x.LanguageId == readyLanguage.Id).Value,
                    LanguageId = languageId,
                    JobExtraBenefitId = x.Id
                }).ToList();
            }

            var createTranslationModel = new AddTranslationToJobViewModel
            {
                JobId = jobId,
                LanguageIcon = language!.IconUrl,
                LanguageId = languageId,
                ReadyLanguageIcon = readyLanguage!.IconUrl,
                TranslationCreateViewModel = new JobTranslationCreateViewModel
                {
                    JobId = jobId,
                    LanguageId = languageId,
                    LanguageIcon = language!.IconUrl,
                },
                ExtraBenefits = benefitModels,
                Responsibilities = responsibilityModels,
            };

            return createTranslationModel;
        }

        public async Task<bool> AddTranslationToJob(int jobId, AddTranslationToJobViewModel model)
        {
            var translationResult = await _jobTranslationService.CreateJobTranslation(jobId, model.TranslationCreateViewModel);
            if (model.Responsibilities.Any())
            {
                foreach (var responsibility in model.Responsibilities)
                {
                    await _jobResponsibilityTranslationService.AddTranslationToResponsibility(responsibility);
                }
            }

            if (model.ExtraBenefits.Any())
            {
                foreach(var benefit in model.ExtraBenefits)
                {
                    await _jobExtraBenefitTranslationService.AddTranslationToBenefit(benefit);
                }
            }

            if (!translationResult)
                return false;

            return true;
        }

        public async Task<JobCreateViewModel> GetJobCreateViewModelAsync(int companyId, int languageId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var translations = await _companyTranslationService.GetAllAsync(predicate: x => x.CompanyId == companyId);
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
                TranslationCreateViewModel = new JobTranslationCreateViewModel
                {
                    LanguageId = languageId,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == languageId) != null ?
                    languages.FirstOrDefault(l => l.Id == languageId)!.IconUrl : "",
                }
            };

            return model;
        }

        public async Task<JobCreateAllViewModel> GetJobCreateAllViewModelAsync(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var translations = await _companyTranslationService.GetAllAsync(predicate: x => x.CompanyId == companyId);
            var addressesList = await _addressService.GetAddressSelectListItems(companyId, language.Id);
            var jobCategoriesList = await _jobCategoryService.GetJobCategorySelectListItems(language.Id);
            var languages = await _languageService.GetAllAsync();
            var model = new JobCreateAllViewModel
            {
                CompanyId = companyId,
                AddressesList = addressesList,
                JobCategoriesList = jobCategoriesList,
                JobTypeListItems = _enumService.GetJobTypeListItems(),
                GenderListItems = _enumService.GetGenderListItems(),
                SalaryTypeListItems = _enumService.GetSalaryTypeListItems(),
                RequiredEducationTypeListItems = _enumService.GetEducationTypeListItems(),
                TranslationCreateViewModels = translations.Select(x => new JobTranslationCreateViewModel
                {
                    LanguageId = x.LanguageId,
                    LanguageIcon = languages.FirstOrDefault(l => l.Id == x.LanguageId) != null ?
                    languages.FirstOrDefault(l => l.Id == x.LanguageId)!.IconUrl : "",
                }).ToList()
            };

            return model;
        }

        public async Task<bool> CreateJobAll(int companyId, JobCreateAllViewModel model)
        {
            var job = new Job();
            job.MinSalary = model.MinSalary;
            job.MaxSalary = model.MaxSalary;
            job.ExpirationDate = model.ExpirationDate;
            job.JobCategoryId = model.JobCategoryId;
            job.AddressId = model.AddressId;
            job.CompanyId = companyId;
            job.Gender = (Gender)model.GenderId;
            job.JobType = (JobType)model.JobTypeId;
            job.SalaryTypeDuration = (SalaryTypeDuration)model.SalaryTypeId;
            job.RequiredMinEducationType = (EducationType)model.RequiredEducationTypeId;
            job.IsActive = true;

            job.Responsibilities = model.Responsibilities.Select(x => Mapper.Map<JobResponsibility>(x)).ToList();
            job.ExtraBenefits = model.ExtraBenefits.Select(x => Mapper.Map<JobExtraBenefit>(x)).ToList();

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

            if (createdJob != null && model.TranslationCreateViewModel!=null)
            {
                
                    var translationResult = await _jobTranslationService.CreateJobTranslation(createdJob.Id, model.TranslationCreateViewModel);

                    if (!translationResult)
                    {
                        await Repository.DeleteAsync(createdJob);
                        return false;
                    }
                
            }
            return true;
        }

        public override async Task<bool> UpdateAsync(int id, JobUpdateViewModel model)
        {
            model.JobType = (JobType)model.JobTypeId;
            model.Gender = (Gender)model.GenderId;
            model.RequiredMinEducationType = (EducationType)model.RequiredEducationTypeId;
            model.SalaryType = (SalaryTypeDuration)model.SalaryTypeId;

            return await base.UpdateAsync(id, model);
        }

        public async Task<PagedJobsOfCompanyViewModel> GetPagedJobsOfCompanyModel(JobFilterViewModel filter, int companyId)
        {
            filter ??= new JobFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedJobsOfCompany = await GetPagedJobsOfCompanyAsync(filter, companyId);

            var model = new PagedJobsOfCompanyViewModel
            {
                Jobs = pagedJobsOfCompany,
                Filter = filter
            };

            return model;
        }

        public async Task<List<JobViewModel>> GetActiveJobsOfCompanyAsync(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();

            var jobs = await Repository.GetAllAsync(predicate: x => x.CompanyId == companyId && !x.IsDeleted 
            && x.IsActive,
                include: x => x.Include(t => t.JobTranslations.Where(x => x.LanguageId == language.Id)));

            var jobViewModels = new List<JobViewModel>();

            foreach (var job in jobs)
            {
                if (job.JobTranslations.Count() != 0)
                {
                    var model = await MapToJobViewModel(job, language.Id);
                    foreach (var readyLang in model.ReadyLanguages)
                    {
                        if (readyLang.Id == language.Id)
                            jobViewModels.Add(model);
                    }
                }

            }

            return jobViewModels;
        }

        public async Task<IEnumerable<JobViewModel>> GetAllJobsAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();
            int languageId = language.Id;

            var jobs = await Repository.GetAllAsync(
                predicate: x => !x.IsDeleted,
                include: x => x
               .Include(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
               .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
               .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
               .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
               .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
                );

            var jobViewModels = new List<JobViewModel>();
           
            foreach(var job in jobs)
            {
                if (job.JobTranslations.Count() != 0)
                {
                    var model = await MapToJobViewModel(job, languageId);
                    foreach (var lang in languages)
                    {
                        if (job.JobTranslations.FirstOrDefault(x => x.LanguageId == language.Id) != null)
                            model.ReadyLanguages.Add(lang);
                        else
                            model.EmptyLanguages.Add(lang);
                    }
                    jobViewModels.Add(model);
                }
            }

            return jobViewModels;
        }

        public override async Task<JobViewModel?> GetByIdAsync(int id)
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;

            var job = await Repository.GetAsync(predicate: x => !x.IsDeleted && x.Id == id,
                include: x => x
                .Include(x=>x.JobApplications)
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

            var jobViewModel = await MapToJobViewModel(job, languageId);
            return jobViewModel;
        }

        public override async Task<JobViewModel> GetAsync(Expression<Func<Job, bool>> predicate, Func<IQueryable<Job>, IIncludableQueryable<Job, object>>? include = null, bool AsNoTracking = false)
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;

            var job = await Repository.GetAsync(predicate: x => !x.IsDeleted,
                include: x => x
                .Include(x=>x.JobApplications)
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

            var jobViewModel = await MapToJobViewModel(job, languageId);
            return jobViewModel;
        }

        public async Task<IEnumerable<JobViewModel>> GetAllWithLanguageAsync(int languageId)
        {
            var jobs = await Repository.GetAllAsync(predicate: x => !x.IsDeleted,
                include: x => x
                .Include(x=>x.JobApplications)
                .Include(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == languageId))
                .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == languageId))
                .Include(a => a.Address!).ThenInclude(a => a.AddressTranslations.Where(x => x.LanguageId == languageId))
                 );

            var jobViewModels = new List<JobViewModel>();
            foreach(var job in jobs)
            {
                var model = await MapToJobViewModel(job, languageId);
                jobViewModels.Add(model);
            }
            return jobViewModels;
        }

        public async Task<PagedResultModel<JobViewModel>> GetPagedJobsOfCompanyAsync(JobFilterViewModel filter, int companyId)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            Expression<Func<Job, bool>> predicate = BuildPredicateCompanyDashboard(filter, language.Id, companyId);
            Func<IQueryable<Job>, IOrderedQueryable<Job>> orderBy = BuildOrderBy(filter, language.Id);
            var pagedJobs = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: orderBy,
                include: x => x
                .Include(x => x.JobApplications)
                .Include(x => x.JobTranslations)
                .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Address!).ThenInclude(x => x.AddressTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.CityTranslations)
                .Include(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations)
                , index: filter.Index, size: filter.Size);

            var jobViewModels = new List<JobViewModel>();
            foreach (var item in pagedJobs.Items)
            {
                var model = await MapToJobViewModel(item, language.Id);
                foreach(var readyLang in model.ReadyLanguages)
                {
                    if (readyLang.Id == language.Id)
                        jobViewModels.Add(model);
                }
            }

            var pagedJobModels = new PagedResultModel<JobViewModel>
            {
                Items = jobViewModels,
                Index = pagedJobs.Index,
                Size = pagedJobs.Size,
                Count = jobViewModels.Count(),
                Pages = pagedJobs.Pages,
            };

            return pagedJobModels;
        }

        public async Task<PagedResultModel<JobViewModel>> GetPagedJobsAsync(JobFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();

            Expression<Func<Job, bool>> predicate = BuildPredicate(filter, language.Id);
            Func<IQueryable<Job>, IOrderedQueryable<Job>> orderBy = BuildOrderBy(filter, language.Id);
            var pagedJobs = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy: orderBy,
                include: x => x
                .Include(x => x.JobApplications)
                .Include(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.JobCategory!).ThenInclude(x => x.JobCategoryTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Responsibilities).ThenInclude(t => t.JobResponsibilityTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.MainDuties).ThenInclude(t => t.JobMainDutyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.ExtraBenefits).ThenInclude(t => t.JobExtraBenefitTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Company!).ThenInclude(t => t.CompanyTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Address!).ThenInclude(x => x.AddressTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.CityTranslations)
                .Include(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations)
                , index: filter.Index, size: filter.Size);

            var jobViewModels = new List<JobViewModel>();
            foreach (var item in pagedJobs.Items)
            {
                var model = await MapToJobViewModel(item, language.Id);
                foreach (var readyLang in model.ReadyLanguages)
                {
                    if (readyLang.Id == language.Id)
                        jobViewModels.Add(model);
                }
            }
            var pagedJobModels = new PagedResultModel<JobViewModel>
            {
                Items = jobViewModels,
                Index = pagedJobs.Index,
                Size = pagedJobs.Size,
                Count = jobViewModels.Count(),
                Pages = pagedJobs.Pages,
            };

            return pagedJobModels;
        }

        public async Task<Dictionary<int, int>> GetJobCountGender()
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobs = await Repository.GetAllAsync(predicate: x => !x.IsDeleted && x.IsActive &&
            x.ExpirationDate>DateTime.UtcNow && 
            x.JobTranslations.FirstOrDefault(t=>t.LanguageId==language.Id)!=null);
            var result = jobs.GroupBy(x => (int)x.Gender).ToDictionary(g => g.Key, g => g.Count());

            return result;
        }

        public async Task<Dictionary<int, int>> GetJobCountJobType()
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobs = await Repository.GetAllAsync(predicate: x => !x.IsDeleted && x.IsActive &&
            x.ExpirationDate > DateTime.UtcNow &&
            x.JobTranslations.FirstOrDefault(t => t.LanguageId == language.Id) != null);
            var result = jobs.GroupBy(x => (int)x.JobType).ToDictionary(g => g.Key, g => g.Count());

            return result;
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
            model.SalaryTypeListItems = _enumService.GetSalaryTypeListItems();
            model.JobTypeListItems = _enumService.GetJobTypeListItems();
            model.AddressesList = addressesList;
            model.JobCategoriesList = jobCategoriesList;
            foreach (var translation in model.JobTranslations)
            {
                var languageT = languages.FirstOrDefault(x => x.Id == translation.LanguageId);
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

        public async Task<bool> CheckHasExpired(int jobId)
        {
            var job = await Repository.GetByIdAsync(jobId);
            if (job!.ExpirationDate < DateTime.UtcNow)
                return true;

            return false;
        }

        private Expression<Func<Job, bool>> BuildPredicateCompanyDashboard(JobFilterViewModel filter, int languageId, int companyId)
        {
            Expression<Func<Job, bool>> predicate = x => !x.IsDeleted && x.CompanyId == companyId &&
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
            (filter.GenderIds == null || filter.GenderIds.Count == 0 ||
            filter.GenderIds.Contains((int)x.Gender)));

            return predicate;
        }

        private Expression<Func<Job, bool>> BuildPredicate(JobFilterViewModel filter, int languageId)
        {
            Expression<Func<Job, bool>> predicate = x => !x.IsDeleted && x.IsActive &&
            x.ExpirationDate>DateTime.UtcNow &&
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
            (filter.GenderIds == null || filter.GenderIds.Count == 0 ||
            filter.GenderIds.Contains((int)x.Gender)));

            return predicate;
        }

        private Func<IQueryable<Job>, IOrderedQueryable<Job>> BuildOrderBy(JobFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "posteddate";
            var sortOrder = "desc"; // default

            // Handle compound sort values (e.g., "Title_asc")
            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                sortOrder = parts[1];
            }
            else if (!string.IsNullOrEmpty(filter.SortOrder))
            {
                sortOrder = filter.SortOrder.ToLower();
            }

            return queryable =>
            {
                IOrderedQueryable<Job> ordered = sortBy switch
                {
                    "title" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.JobTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault())
                        : queryable.OrderByDescending(x => x.JobTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()),

                    "applicants" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.JobApplications.Count)
                        : queryable.OrderByDescending(x => x.JobApplications.Count),

                    "salary" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.MinSalary)
                        : queryable.OrderByDescending(x => x.MaxSalary),

                    // Default: sort by posted date
                    _ => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt)
                };

                return ordered;
            };
        }

        private async Task<JobViewModel> MapToJobViewModel(Job jobEntity, int languageId)
        {
            bool expired = false ;
            if (await CheckHasExpired(jobEntity.Id))
                expired = true;
           
            var jobViewModel = new JobViewModel
            {
                Id = jobEntity.Id,
                Title = jobEntity.JobTranslations.FirstOrDefault(x => x.LanguageId ==languageId)?.Title,
                Description = jobEntity.JobTranslations.FirstOrDefault(x=>x.LanguageId==languageId)?.Description,
                RequiredExperience = jobEntity.JobTranslations.FirstOrDefault(x => x.LanguageId == languageId)?.RequiredExperience,
                VacancyCount = jobEntity.VacancyCount,
                ApplicationCount = jobEntity.JobApplications.Where(x => x.JobId == jobEntity.Id && x.JobStatus != (JobApplicationStatus)5).Count(),
                InterviewCount = jobEntity.JobApplications.Where(x => x.JobId == jobEntity.Id && x.JobStatus == (JobApplicationStatus)2).Count(),
                RejectedApplicationCount = jobEntity.JobApplications.Where(x => x.JobId == jobEntity.Id && x.JobStatus == (JobApplicationStatus)4).Count(),
                AcceptedApplicationCount = jobEntity.JobApplications.Where(x => x.JobId == jobEntity.Id && x.JobStatus == (JobApplicationStatus)3).Count(),
                MinSalary = jobEntity.MinSalary,
                MaxSalary = jobEntity.MaxSalary,
                IsActive = jobEntity.IsActive,
                ExpirationDate = jobEntity.ExpirationDate,
                Expired = expired,
                CreatedAt = jobEntity.CreatedAt,
                JobCategoryId = jobEntity.JobCategoryId,
                Gender = jobEntity.Gender.ToString(),
                RequiredMinEducationType = jobEntity.RequiredMinEducationType.ToString(),
                SalaryTypeDuration = jobEntity.SalaryTypeDuration.ToString(),
                JobType = jobEntity.JobType.ToString(),
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
                CompanyImages = jobEntity.Company!.CompanyImages.Select(x => x.ImageUrl).ToList()
            };

            var languages = await _languageService.GetAllAsync();

            foreach (var translation in jobEntity.JobTranslations)
            {
                foreach (var lang in languages)
                {
                    if (translation.LanguageId == lang.Id)
                        jobViewModel.ReadyLanguages.Add(lang);
                }
            }

            foreach(var lang in languages)
            {
                if (!jobViewModel.ReadyLanguages.Contains(lang))
                    jobViewModel.EmptyLanguages.Add(lang);
            }

            return jobViewModel;
        }

    }
}
