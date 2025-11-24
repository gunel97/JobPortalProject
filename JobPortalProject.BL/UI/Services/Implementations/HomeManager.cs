using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class HomeManager:IHomeService
    {
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICookieService _cookieService;

        public HomeManager(IJobCategoryService jobCategoryService, ICookieService cookieService)
        {
            _jobCategoryService = jobCategoryService;
            _cookieService = cookieService;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobCategories = await _jobCategoryService.GetAllAsync(
                                                predicate: x=>!x.IsDeleted,
                                                include: c => c
                                               .Include(ct => ct.JobCategoryTranslations!
                                               .Where(j => j.LanguageId == language.Id)));

            var homeViewModel = new HomeViewModel
            {
                JobCategories = jobCategories.ToList()
            };

            return homeViewModel;
        }


    }
}
