using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IAddressService _addressService;
        private readonly ICompanyService _companyService;
        private readonly IJobService _jobService;
        private readonly ICandidateService _candidateService;
        private readonly IJobApplicationService _jobApplicationService;

        public HomeManager(IJobCategoryService jobCategoryService, ICookieService cookieService, IAddressService addressService, ICompanyService companyService, IJobService jobService, ICandidateService candidateService, IJobApplicationService jobApplicationService)
        {
            _jobCategoryService = jobCategoryService;
            _cookieService = cookieService;
            _addressService = addressService;
            _companyService = companyService;
            _jobService = jobService;
            _candidateService = candidateService;
            _jobApplicationService = jobApplicationService;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            var language = await _cookieService.GetLanguageAsync();
            var jobCategories = await _jobCategoryService.GetAllAsync(predicate: x => x.Jobs.Count != 0 && !x.IsDeleted,
                   include: x => x
                   .Include(x => x.JobCategoryTranslations.Where(x => x.LanguageId == language.Id))
                   .Include(x => x.Jobs).ThenInclude(x => x.JobTranslations.Where(t => t.LanguageId == language.Id))
                   );
            var addresses = await _addressService.GetCompaniesAddressesAsync(language.Id);
            var jobs = await _jobService.GetAllJobsAsync();
            var jobModels = jobs.ToList();
            var jobCategoryListItems = await _jobCategoryService.GetJobCategorySelectListItems(language.Id);
            var companies = await _companyService.GetAllCompaniesAsync();
            var candidates = await _candidateService.GetAllAsync(predicate: x => x.Resume != null && !x.IsDeleted);
            foreach (var job in jobModels)
            {
                if (await _jobApplicationService.CheckIfJobApplied(job.Id))
                    job.IsApplied = true;
            }

            var listItems = new List<SelectListItem>();
            foreach (var jobCategory in jobCategories)
            {
                if (jobCategory.JobIds.Count() != 0)
                    listItems.Add(new SelectListItem(jobCategory.Name, jobCategory.Id.ToString()));
                foreach (var id in jobCategory.JobIds)
                {
                    if (jobs.FirstOrDefault(x => x.Id == id) != null &&
                        jobs.FirstOrDefault(x => x.Id == id).ReadyLanguages.Contains(language))
                        jobCategory.JobIds.Remove(id);
                }
            }

            var addressesByCities = addresses.DistinctBy(a => a.CityName!);
            var addressesCitiesGroup = addresses.GroupBy(a => a.CityName).ToList();

        
            var homeViewModel = new HomeViewModel
            {
                JobCategories = jobCategories.Where(x => x.JobIds.Any()).ToList(),
                Addresses = addressesCitiesGroup,
                Companies = companies.OrderByDescending(x=>x.LastPostedJob).Take(6).ToList(),
                CandidateCount=candidates.Count(),
                ActiveCompanyCount = companies.Count(),
                ActiveJobCount = jobModels.Where(x => !x.Expired && x.IsActive).Count(),
                NewJobCount = jobModels.Where(x => !x.Expired && x.IsActive && x.CreatedAt > DateTime.UtcNow.AddDays(-10)).Count(),
                Jobs = jobModels.Where(x=>x.IsActive && x.ExpirationDate>DateTime.UtcNow).OrderByDescending(j => j.CreatedAt).Take(6).ToList(),
                JobCategoryListItems = listItems
            };

            return homeViewModel;
        }
    }
}
