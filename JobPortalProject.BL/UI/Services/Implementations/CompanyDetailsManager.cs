using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Implementations
{
    public class CompanyDetailsManager : ICompanyDetailsService
    {
        private readonly ICookieService _cookieService;
        private readonly ICompanyService _companyService;
        private readonly IAddressService _addressService;
        private readonly ICompanySocialService _companySocialService;
        private readonly IJobService _jobService;
        private readonly ICandidateService _candidateService;
        private readonly IJobApplicationService _jobApplicationService;

        public CompanyDetailsManager(ICompanyService companyService, ICookieService cookieService, IAddressService addressService, ICompanySocialService companySocialService, IJobService jobService, ICandidateService candidateService, IJobApplicationService jobApplicationService)
        {
            _companyService = companyService;
            _cookieService = cookieService;
            _addressService = addressService;
            _companySocialService = companySocialService;
            _jobService = jobService;
            _candidateService = candidateService;
            _jobApplicationService = jobApplicationService;
        }

        public async Task<CompanyDetailsViewModel> GetCompanyDetailsAsync(int id, int languageId)
        {
            var addresses = await _addressService.GetAllAsync();

            var company = await _companyService.GetAsync(
                                            predicate: x => !x.IsDeleted && x.Id == id,
                                            include: x => x
                                            .Include(ct => ct.CompanyTranslations!.Where(x => x.LanguageId == languageId))
                                            .Include(x=>x.Jobs)
                                            .Include(x=>x.Addresses).ThenInclude(x=>x.AddressTranslations.Where(x=>x.LanguageId==languageId))
                                            .Include(t => t.CompanyType!).ThenInclude(ct => ct.CompanyTypeTranslations!.Where(x => x.LanguageId == languageId))
                                            .Include(w => w.WorkingFields).ThenInclude(wt => wt.Translations.Where(x => x.LanguageId == languageId)));

            if (company.TranslationsCount == 0)
                return null!;

            var companySocials = await _companySocialService.GetAllAsync(
                                            predicate: x => !x.IsDeleted && x.CompanyId == id,
                                            include: x => x
                                            .Include(s => s.SocialMedia!));

            var jobs = await _jobService.GetActiveJobsOfCompanyAsync(company.Id);
            var activeJobModels = jobs.Where(x=>!x.Expired).ToList();
            var candidate = await _candidateService.GetCandidate();
            if (candidate != null)
            {
                var appliedIds = new List<int>();
                var appliedJobs = await _jobApplicationService.GetAppliedJobsOfCandidate(candidate.Id);
                appliedJobs.ForEach(x=>  appliedIds.Add(x.JobId));

                foreach(var job in activeJobModels)
                {
                    if (appliedIds.Contains(job.Id))
                        job.IsApplied = true;
                }
            }

            var allJobs = (await _jobService.GetAllAsync(predicate: x => x.CompanyId == company.Id && !x.IsDeleted && x.IsActive))
                .ToList();
            if (allJobs.Any()) {
                company.LastPostedJob = allJobs.OrderByDescending(x => x.CreatedAt).Take(1).FirstOrDefault().CreatedAt;
            }

            var readyLanguages = await _companyService.GetReadyLanguagesOfCompany(id);
            var companyDetailsViewModel = new CompanyDetailsViewModel
            {
                Company = company,
                CompanySocials = companySocials.ToList(),
                ActiveJobs = activeJobModels.OrderBy(x=>x.CreatedAt).ToList(),
                ReadyLanguages=readyLanguages
            };

            return companyDetailsViewModel;

        }
    }
}
