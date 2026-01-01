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
            var addresses =await  _addressService.GetAllAsync();
            var jobs = await _jobService.GetAllJobsAsync();
            var candidate = await _candidateService.GetCandidate();
            var jobIds = new List<int>();
            var jobCategoryListItems = await _jobCategoryService.GetJobCategorySelectListItems(language.Id);

            if (candidate != null)
            {
                var appliedJobs = await _jobApplicationService.GetAppliedJobsOfCandidate(candidate.Id);
                appliedJobs.ForEach(x => jobIds.Add(x.JobId));
            }

            var jobCategories = await _jobCategoryService.GetAllAsync(
                                                predicate: x => !x.IsDeleted,
                                                include: c => c
                                                .Include(x=>x.Jobs)
                                                .Include(ct => ct.JobCategoryTranslations!
                                                .Where(j => j.LanguageId == language.Id)));

            var addressesByCities = addresses.DistinctBy(a => a.City!.Name!);

            var companies = await _companyService.GetAllAsync(
                                               include: c => c
                                               .Include(ct => ct.CompanyTranslations!
                                               .Where(c => c.LanguageId == language.Id)));

            var homeViewModel = new HomeViewModel
            {
                JobCategories = jobCategories.Where(x => x.JobIds.Any()).ToList(),
                Addresses = addressesByCities.ToList(),
                Companies = companies.ToList(),
                Jobs = jobs.OrderByDescending(j => j.CreatedAt).Take(6).ToList(),
                AppliedJobIds = jobIds,
                JobCategoryListItems=jobCategoryListItems
            };

            return homeViewModel;
        }


    }
}
