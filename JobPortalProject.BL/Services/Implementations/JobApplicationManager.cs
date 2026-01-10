using AutoMapper;
using CloudinaryDotNet.Actions;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobApplicationManager : CrudManager<JobApplication, JobApplicationsOfCandidateViewModel, JobApplicationCreateViewModel, JobApplicationUpdateViewModel>
        , IJobApplicationService
    {
        private readonly ICookieService _cookieService;
        private readonly ICandidateService _candidateService;
        private readonly IJobService _jobService;
        private readonly IResumeService _resumeService;

        public JobApplicationManager(IRepositoryAsync<JobApplication> repository, IMapper mapper, ICookieService cookieService, ICandidateService candidateService, IJobService jobService, IResumeService resumeService, IPersonalInfoService personalInfoService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _candidateService = candidateService;
            _jobService = jobService;
            _resumeService = resumeService;
        }

        public async Task<string> CheckJobStatus(int jobId)
        {
            var application = await Repository.GetAsync(predicate: x => x.JobId == jobId);
            return application!.JobStatus.ToString();
        }
        public async Task<bool> CheckIfJobApplied(int jobId)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null)
                return false;

            var appliedJobs = await GetAppliedJobsOfCandidate(candidate.Id);
            var appliedIds = new List<int>();
            appliedJobs.ForEach(x => appliedIds.Add(x.JobId));

            if (appliedIds.Contains(jobId))
                return true;
            else
                return false;
        }

        public async Task<List<JobApplication>> GetAppliedJobsOfCandidate(int candidateId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languageId = language.Id;
            var appliedJobs = await Repository.GetAllAsync(predicate: x => x.CandidateId == candidateId,
              include: x => x
              .Include(x => x.Job).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Company).ThenInclude(x => x.CompanyTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.CityTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId)));

            return appliedJobs.ToList();
        }

        public async Task<List<JobApplicationsOfCandidateViewModel>> GetAppliedJobModelsOfCandidate(int candidateId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobApplicationEntities = await GetAppliedJobsOfCandidate(candidateId);
            var models = new List<JobApplicationsOfCandidateViewModel>();

            if (jobApplicationEntities.Any() && jobApplicationEntities.Count() > 0)
            {
                foreach (var entity in jobApplicationEntities)
                {
                    if (entity.Job!=null && entity.Job.JobTranslations.Any(x => x.LanguageId ==language.Id)) 
                    {
                        var model = MapToJobApplicationsOfCandidateViewModel(entity);
                        if (model != null)
                            models.Add(model);
                    }
                }
            }

            return models;
        }

        public async Task<PagedResultModel<JobApplicationsOfCandidateViewModel>> GetPagedAppliedJobsOfCandidate(JobApplicationsOfCandidateFilterViewModel filter, int candidateId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languageId = language.Id;
            Expression<Func<JobApplication, bool>> predicate = BuildPredicate(filter, language.Id, candidateId);
            Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>> orderBy = BuildOrderBy(filter, language.Id);

            var pagedJobApplications = await Repository.GetPagedListAsync(
                predicate: predicate,
                orderBy:orderBy,
                include: x=>x
              .Include(x => x.Job).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Company).ThenInclude(x => x.CompanyTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.CityTranslations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.Address).ThenInclude(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId)));

            var models = new List<JobApplicationsOfCandidateViewModel>();
            foreach(var item in pagedJobApplications.Items)
            {
                if (item.Job != null && item.Job.JobTranslations.Any(x => x.LanguageId == language.Id))
                {
                    var model = MapToJobApplicationsOfCandidateViewModel(item);
                    if (model != null)
                        models.Add(model);
                }
            }

            var pagedModel = new PagedResultModel<JobApplicationsOfCandidateViewModel>
            {
                Items = models,
                Index = pagedJobApplications.Index,
                Size = pagedJobApplications.Size,
                Count = models.Count,
                Pages = pagedJobApplications.Pages,
            };

            return pagedModel;
        }
      
        public async Task<PagedResultModel<ApplicantOfJobViewModel>> GetPagedApplicantsOfJob(int jobId, ApplicantsOfJobFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languageId = language.Id;
            Expression<Func<JobApplication, bool>> predicate = BuildPredicateApplicantsOfJob(filter, language.Id, jobId);
            Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>> orderBy = BuildOrderByApplicantsOfJob(filter, language.Id);

            var applications = await Repository.GetPagedListAsync(predicate: predicate,
                orderBy:orderBy,
              include: x => x
              .Include(x => x.Candidate).ThenInclude(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Candidate).ThenInclude(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == languageId)),
              index: filter.Index,
              size: filter.Size);

            

            var model = new PagedResultModel<ApplicantOfJobViewModel>
            {
                Index = applications.Index,
                Size=applications.Size,
                Count=applications.Items.Count(),
                Pages=applications.Pages
            };

            foreach(var application in applications.Items)
            {
                model.Items.Add(await MapToApplicantsOfJobViewModel(application));
            }

            return model;
        }

        public async Task<bool> CancelJobApplication(int jobId, int candidateId)
        {
            var jobApplication = await Repository.GetAsync(predicate: x => x.JobId == jobId && x.CandidateId == candidateId);
            if (jobApplication == null)
                return false;
            jobApplication.JobStatus = (JobApplicationStatus)5;
            var result = await Repository.UpdateAsync(jobApplication);

            if (result == null)
                return false;
            return true;
        }

        public async Task<bool> AcceptJobApplication(int jobId, int candidateId)
        {
            var jobApplication = await Repository.GetAsync(predicate: x => x.JobId == jobId && x.CandidateId == candidateId);
            if (jobApplication == null)
                return false;
            jobApplication.JobStatus = (JobApplicationStatus)3;
            var result = await Repository.UpdateAsync(jobApplication);

            if (result == null)
                return false;
            return true;
        }

        public async Task<bool> RejectJobApplication(int jobId, int candidateId)
        {
            var jobApplication = await Repository.GetAsync(predicate: x => x.JobId == jobId && x.CandidateId == candidateId);
            if (jobApplication == null)
                return false;
            jobApplication.JobStatus = (JobApplicationStatus)4;
            var result = await Repository.UpdateAsync(jobApplication);

            if (result == null)
                return false;
            return true;
        }

        public async Task<bool> InterviewJobApplication(int jobId, int candidateId)
        {
            var jobApplication = await Repository.GetAsync(predicate: x => x.JobId == jobId && x.CandidateId == candidateId);
            if (jobApplication == null)
                return false;
            jobApplication.JobStatus = (JobApplicationStatus)2;
            var result = await Repository.UpdateAsync(jobApplication);

            if (result == null)
                return false;
            return true;
        }

        public async Task<bool> ApplyJob(int jobId, int candidateId)
        {
            var appliedJobs = await GetAppliedJobsOfCandidate(candidateId);
            foreach (var appliedJob in appliedJobs)
            {
                if (appliedJob.Id == jobId)
                    return false;
            }

            var model = new JobApplicationCreateViewModel
            {
                CandidateId = candidateId,
                JobId = jobId,
            };

            var result = await CreateAsync(model);

            if (result == null)
                return false;

            return true;
        }

        public async Task<List<ApplicantOfCompanyViewModel>> GetApplicantsOfCompany(int companyId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobApplications = await Repository.GetAllAsync(
                predicate: x => x.JobStatus != (JobApplicationStatus)5 && !x.IsDeleted && x.Job.CompanyId==companyId,
                include: x => x
                .Include(x => x.Job).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Job).ThenInclude(x => x.Company).ThenInclude(x => x.CompanyTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x=>x.Candidate).ThenInclude(x=>x.Resume).ThenInclude(x=>x.Translations.Where(t=>t.LanguageId==language.Id))
                .Include(x=>x.Candidate).ThenInclude(x=>x.Resume).ThenInclude(x=>x.PersonalInfo).ThenInclude(x=>x.Translations.Where(t=>t.LanguageId==language.Id)));

            var models = new List<ApplicantOfCompanyViewModel>();

            foreach(var application in jobApplications)
            {
                if (application.Job!=null && application.Job.JobTranslations.Count() != 0)
                {
                    var model = new ApplicantOfCompanyViewModel
                    {
                        ApplicationId = application.Id,
                        CandidateId = application.CandidateId,
                        JobId = application.JobId,
                        JobTitle = application.Job.JobTranslations.FirstOrDefault().Title,
                        CandidateName = application.Candidate.Resume.PersonalInfo.Translations.FirstOrDefault().FirstName + " " +
                        application.Candidate.Resume.PersonalInfo.Translations.FirstOrDefault().LastName,
                        BirthDateOfCandidate = application.Candidate.Resume.PersonalInfo.BirthDate,
                        AppliedAt = application.CreatedAt,
                        ExpireAt = application.Job.ExpirationDate,
                        PostedAt = application.Job.CreatedAt,
                        Status = application.JobStatus.ToString(),
                        ImageUrl = application.Candidate.Resume.PersonalInfo.ImageUrl
                    };

                    models.Add(model);
                }
            }

            return models;
        }

        public async Task<ApplicantsOfJobPagedViewModel> GetPagedApplicantsViewModel(int jobId, ApplicantsOfJobFilterViewModel filter)
        {
            var job = await _jobService.GetByIdAsync(jobId);
            if (job == null)
                return null!;

            filter ??= new ApplicantsOfJobFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var language = await _cookieService.GetLanguageAsync();
            var applications = await GetPagedApplicantsOfJob(jobId, filter);

            var model = new ApplicantsOfJobPagedViewModel();

            model.JobTitle = job.Title;
            if (job.CreatedAt.HasValue)
                model.JobPostedDate = job.CreatedAt.Value;
            if (job.ExpirationDate.HasValue)
                model.JobExpireDate = job.ExpirationDate.Value;
            model.Job = job;
            model.Applications= applications;

            return model;
        }

        public async Task<AppliedJobsOfCandidatePageViewModel> GetAppliedJobsPageOfCandidateViewModel(JobApplicationsOfCandidateFilterViewModel filter, int candidateId)
        {
            var dashboard = await _candidateService.GetDashboardViewModel();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new JobApplicationsOfCandidateFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var jobApplications = await GetPagedAppliedJobsOfCandidate(filter, candidateId);
            var model = new AppliedJobsOfCandidatePageViewModel();
            model.JobApplicationsModels = jobApplications;

            model.Dashboard = dashboard;
            model.Filter = filter;
            return model;
        }
        //
        private Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>> BuildOrderByApplicantsOfJob(ApplicantsOfJobFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "appliedat";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                sortOrder = parts[1];
            }

            return queryable =>
            {
                IOrderedQueryable<JobApplication> ordered = sortBy switch
                {
                    "appliedat" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt),

                    _ => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt)
                };

                return ordered;
            };
        }

        private Expression<Func<JobApplication, bool>> BuildPredicateApplicantsOfJob(ApplicantsOfJobFilterViewModel filter, int languageId, int jobİd)
        {
            Expression<Func<JobApplication, bool>> predicate = x => !x.IsDeleted && x.JobId == jobİd && x.JobStatus!=(JobApplicationStatus)5;

            return predicate;
        }


        private async Task<ApplicantOfJobViewModel> MapToApplicantsOfJobViewModel(JobApplication entity)
        {
            if (entity.Candidate == null || entity.Candidate.Resume == null || entity.Candidate.Resume.PersonalInfo==null || entity.Job==null)
                return null!;

            var resume = await _resumeService.GetResume(entity.Candidate.Resume.Id);

            if (entity.Job.ExpirationDate<DateTime.UtcNow)
                entity.JobStatus = (JobApplicationStatus)6;

            var model = new ApplicantOfJobViewModel
            {
                JobApplicationId = entity.Id,
                CandidateId=entity.Candidate.Id,
                CandidateName = entity.Candidate.Resume.PersonalInfo.Translations.FirstOrDefault()!.FirstName + " " +
                entity.Candidate.Resume.PersonalInfo.Translations.FirstOrDefault()!.LastName,
                CandidateImageUrl = entity.Candidate.Resume.PersonalInfo.ImageUrl,
                CandidateBirthDate = entity.Candidate.Resume.PersonalInfo.BirthDate,
                ApplyDate = entity.CreatedAt,
                Status = entity.JobStatus.ToString(),
                ResumeModel = resume,
            };

            return model;
        }

        private JobApplicationsOfCandidateViewModel MapToJobApplicationsOfCandidateViewModel(JobApplication entity)
        {
            if (entity.Job == null || !entity.Job.JobTranslations.Any() || entity.Job.Company==null)
                return null!;

            if (entity.Job.ExpirationDate < DateTime.Now)
                entity.JobStatus=(JobApplicationStatus)6;

            var model = new JobApplicationsOfCandidateViewModel
            {
                JobApplicationId = entity.Id,
                JobId = entity.JobId,
                CompanyId=entity.Job.CompanyId,
                CandidateId = entity.CandidateId,
                JobTitle = entity.Job.JobTranslations.FirstOrDefault()!.Title,
                JobAddress = entity.Job.Address?.City?.CityTranslations.FirstOrDefault()!.Name + ", " +
                entity.Job.Address?.City?.Country?.Translations.FirstOrDefault()!.Name,
                CompanyName = entity.Job.Company.CompanyTranslations.FirstOrDefault()!.Name,
                CompanyLogo=entity.Job.Company.LogoUrl,
                MinSalary = entity.Job.MinSalary,
                MaxSalary = entity.Job.MaxSalary,
                SalaryType = entity.Job.SalaryTypeDuration.ToString(),
                JobCreatedAt = entity.Job.CreatedAt,
                AppliedAt = entity.CreatedAt,
                Status = entity.JobStatus.ToString(),
            };

            return model;
        }

        private Func<IQueryable<JobApplication>, IOrderedQueryable<JobApplication>> BuildOrderBy(JobApplicationsOfCandidateFilterViewModel filter, int languageId)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "appliedat";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                sortOrder = parts[1];
            }

            return queryable =>
            {
                IOrderedQueryable<JobApplication> ordered = sortBy switch
                {
                    "title" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.Job.JobTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault())
                        : queryable.OrderByDescending(x => x.Job.JobTranslations
                            .Where(t => t.LanguageId == languageId)
                            .Select(t => t.Title)
                            .FirstOrDefault()),

                    "appliedat" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt),

                    "createdat" => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.Job.CreatedAt)
                        : queryable.OrderByDescending(x => x.Job.CreatedAt),

                    _ => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt)
                };

                return ordered;
            };
        }

        private Expression<Func<JobApplication, bool>> BuildPredicate(JobApplicationsOfCandidateFilterViewModel filter, int languageId, int candidateId)
        {
            Expression<Func<JobApplication, bool>> predicate = x => !x.IsDeleted && x.CandidateId==candidateId &&
            (string.IsNullOrEmpty(filter.SearchTerm) ||
            x.Job.JobTranslations.Any(t => t.LanguageId == languageId && t.Title.Contains(filter.SearchTerm) ||
            x.Job.Company.CompanyTranslations.Any(t => t.LanguageId == languageId && t.Name.Contains(filter.SearchTerm))));

            return predicate;
        }


    }

}
