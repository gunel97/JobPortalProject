using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class JobListingManager : IJobListingService
    {
        private readonly IJobService _jobService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICookieService _cookieService;
        private readonly IEnumService _enumService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ILanguageService _languageService;

        public JobListingManager(IJobService jobService, IJobCategoryService jobCategoryService, ICookieService cookieService, IEnumService enumService, IJobApplicationService jobApplicationService, ILanguageService languageService)
        {
            _jobService = jobService;
            _jobCategoryService = jobCategoryService;
            _cookieService = cookieService;
            _enumService = enumService;
            _jobApplicationService = jobApplicationService;
            _languageService = languageService;
        }


        public async Task<PagedJobListingViewModel> GetPagedJobListingViewModel(JobFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();

            filter ??= new JobFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;
            var pagedJobs = await _jobService.GetPagedJobsAsync(filter);
            var jobCategories = await _jobCategoryService.GetAllAsync(predicate: x => x.Jobs.Count != 0,
                include: x => x
                .Include(x => x.JobCategoryTranslations.Where(x => x.LanguageId == language.Id))
                .Include(x => x.Jobs).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
                );

            var jobTypes = _enumService.GetJobTypeListItems();
            var genders = _enumService.GetGenderListItems();
            var jobTypesCounts = await _jobService.GetJobCountJobType();
            var genderCounts = await _jobService.GetJobCountGender();
            double minSalary = 0;
            double maxSalary = 0;

            if (pagedJobs.Items.Any())
            {
                minSalary = pagedJobs.Items.MinBy(x => x.MinSalary).MinSalary;
                maxSalary = pagedJobs.Items.MaxBy(x => x.MaxSalary).MaxSalary;
            }

            foreach (var job in pagedJobs.Items)
            {
                if (job != null)
                {
                    if (await _jobApplicationService.CheckIfJobApplied(job.Id))
                        job.IsApplied = true;
                }
            }

            var jobListingViewModel = new PagedJobListingViewModel
            {
                MinSalary = minSalary,
                MaxSalary = maxSalary,
                Jobs = pagedJobs,
                Filter = filter,
                JobCategories = jobCategories.ToList(),
                JobTypes = jobTypes,
                Genders = genders,
                JobTypeCounts = jobTypesCounts,
                GenderCounts = genderCounts,
            };

            return jobListingViewModel;
        }
    }
}
