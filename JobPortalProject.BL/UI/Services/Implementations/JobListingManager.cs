using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class JobListingManager : IJobListingService
    {
        private readonly IJobService _jobService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICookieService _cookieService;

        public JobListingManager(IJobService jobService, IJobCategoryService jobCategoryService, ICookieService cookieService)
        {
            _jobService = jobService;
            _jobCategoryService = jobCategoryService;
            _cookieService = cookieService;
        }

        //public async Task<JobListingViewModel> GetJobListingViewModel()
        //{
        //    var language = await _cookieService.GetLanguageAsync();
        //    var jobs = await _jobService.GetAllJobsAsync();
        //    var jobCategories = await _jobCategoryService.GetAllAsync(
        //        include: x => x
        //        .Include(x => x.JobCategoryTranslations.Where(x => x.LanguageId == language.Id))
        //        .Include(x=>x.Jobs)
        //        );
        //    var jobTypes = Enum.GetValues(typeof(JobType)).Cast<JobType>().ToList();
        //    var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

        //    var jobListingViewModel = new JobListingViewModel
        //    {
        //        Jobs=jobs.ToList(),
        //        JobCategories=jobCategories.ToList(),
        //        JobTypes=jobTypes.Select(x=>x.ToString()).ToList(),
        //        Genders=genders.Select(x=>x.ToString()).ToList()
        //    };

        //    return jobListingViewModel;
        //}

        public async Task<PagedJobListingViewModel> GetPagedJobListingViewModel(int index=0, int size=10)
        {
            var language = await _cookieService.GetLanguageAsync();
            var pagedJobs = await _jobService.GetPagedJobsAsync(index, size);
            var jobCategories = await _jobCategoryService.GetAllAsync(predicate: x=>x.Jobs.Count!=0,
                include: x => x
                .Include(x => x.JobCategoryTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x=>x.Jobs).ThenInclude(x=>x.JobTranslations.Where(t=>t.LanguageId==language.Id))
                );
            var jobTypes = Enum.GetValues(typeof(JobType)).Cast<JobType>().ToList();
            var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            var jobListingViewModel = new PagedJobListingViewModel
            {
                Jobs=pagedJobs,
                JobCategories=jobCategories.ToList(),
                JobTypes=jobTypes.Select(x=>x.ToString()).ToList(),
                Genders=genders.Select(x=>x.ToString()).ToList()
            };

            return jobListingViewModel;
        }

    }
}
