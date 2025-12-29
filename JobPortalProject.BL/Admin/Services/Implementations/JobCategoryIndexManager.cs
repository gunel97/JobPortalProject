using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<JobCategoryIndexViewModel> GetJobCategoryIndexModel()
        {
            var languages = await _languageService.GetAllAsync();
            var language = await _cookieService.GetLanguageAsync();
            var jobCategories = await _jobCategoryService.GetAllAsync(include:
                x => x.Include(x => x.JobCategoryTranslations.Where(t => t.LanguageId == language.Id))
                .Include(x => x.Jobs));

            var model = new JobCategoryIndexViewModel
            {
                Languages = languages.ToList(),
                JobCategories= jobCategories.ToList(),
            };

            return model;
        }
    }
}
