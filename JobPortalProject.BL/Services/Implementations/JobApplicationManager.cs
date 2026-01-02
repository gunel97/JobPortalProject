using AutoMapper;
using CloudinaryDotNet.Actions;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.DataContext.Enums;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        public async Task<List<JobApplication>> GetApplicantsOfJob(int jobId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languageId = language.Id;
            var appliedJobs = await Repository.GetAllAsync(predicate: x => x.JobId == jobId && x.JobStatus != (JobApplicationStatus)5,
              include: x => x
              .Include(x => x.Candidate).ThenInclude(x => x.Resume).ThenInclude(x => x.PersonalInfo).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Candidate).ThenInclude(x => x.Resume).ThenInclude(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
              .Include(x => x.Job).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == languageId)));

            return appliedJobs.ToList();
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

        public async Task<ApplicantOfJobViewModel> MapToApplicantsOfJobViewModel(JobApplication entity)
        {
            if (entity.Candidate == null || entity.Candidate.Resume == null || entity.Candidate.Resume.PersonalInfo==null)
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
                //JobPostedDate = entity.Job.CreatedAt,
                ApplyDate = entity.CreatedAt,
                //ExpireDate = entity.Job.ExpirationDate,
                Status = entity.JobStatus.ToString(),
                ResumeModel = resume,
            };

            return model;
        }

        public JobApplicationsOfCandidateViewModel MapToJobApplicationsOfCandidateViewModel(JobApplication entity)
        {
            if (entity.Job.ExpirationDate < DateTime.Now)
                entity.JobStatus=(JobApplicationStatus)6;

            var model = new JobApplicationsOfCandidateViewModel
            {
                JobApplicationId = entity.Id,
                JobId = entity.JobId,
                CompanyId=entity.Job.CompanyId,
                CandidateId = entity.CandidateId,
                JobTitle = entity.Job.JobTranslations.FirstOrDefault()!.Title,
                JobAddress = entity.Job.Address.City.CityTranslations.FirstOrDefault()!.Name + ", " +
                entity.Job.Address.City.Country.Translations.FirstOrDefault()!.Name,
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

        public async Task<AppliedJobsOfCandidatePageViewModel> GetAppliedJobsPageOfCandidateViewModel(int candidateId)
        {
            var dashboard = await _candidateService.GetDashboardViewModel();
            var language = await _cookieService.GetLanguageAsync();
            var jobApplications = await GetAppliedJobsOfCandidate(candidateId);

            var model = new AppliedJobsOfCandidatePageViewModel();

            foreach (var jobApplication in jobApplications)
            {
                var expired = await _jobService.CheckHasExpired(jobApplication.JobId);
                if (expired)
                    jobApplication.JobStatus = (JobApplicationStatus)6;
                var jobApplicationModel = MapToJobApplicationsOfCandidateViewModel(jobApplication);
                model.JobApplicationsModels.Add(jobApplicationModel);
            }

            model.Dashboard = dashboard;
            return model;
        }

        public async Task<ApplicantsOfJobViewModel> GetApplicantsViewModel(int jobId)
        {
            var language = await _cookieService.GetLanguageAsync();
            var entities = await GetApplicantsOfJob(jobId);
            var job = await _jobService.GetByIdAsync(jobId);
            if (job == null)
                return null!;
            var model = new ApplicantsOfJobViewModel();
            foreach(var entity in entities)
            {
                var application = await MapToApplicantsOfJobViewModel(entity);
                model.Applications.Add(application);
            }

            model.JobTitle = job.Title;
            if(job.CreatedAt.HasValue)
            model.JobPostedDate = job.CreatedAt.Value;
            if(job.ExpirationDate.HasValue)
            model.JobExpireDate = job.ExpirationDate.Value;
            return model;
        }
    }

}
