using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class JobCategoryIndexManager : IJobCategoryIndexService
    {
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public JobCategoryIndexManager(IJobCategoryService jobCategoryService, ILanguageService languageService, ICookieService cookieService)
        {
            _jobCategoryService = jobCategoryService;
            _languageService = languageService;
            _cookieService = cookieService;
        }

        public async Task<JobCategoryPagedIndexViewModel> GetPagedJobCategoryIndexModel(JobCategoryFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            filter ??= new JobCategoryFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;

            var pagedJobCategories = await _jobCategoryService.GetPagedJobCategoriesAsync(filter);

            var model = new JobCategoryPagedIndexViewModel
            {
                Languages = languages.ToList(),
                Filter=filter,
                JobCategories = pagedJobCategories
            };

            return model;
        }


    }

}
