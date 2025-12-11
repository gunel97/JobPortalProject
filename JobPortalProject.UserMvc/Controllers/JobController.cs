using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace JobPortalProject.UserMvc.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IJobListingService _jobListingService;
        private readonly ICompanyService _companyService;

        public JobController(IJobService jobService, IJobListingService jobListingService, ICompanyService companyService)
        {
            _jobService = jobService;
            _jobListingService = jobListingService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var jobListingViewModel = await _jobListingService.GetJobListingViewModel();

            return View(jobListingViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            int jobId = int.Parse(id.Split('-').Last());

            var job = await _jobService.GetByIdAsync(jobId);

            if (job == null)
                return NotFound();

            return View(job);
        }

        public async Task<IActionResult> JobList()
        {
            var companyId = await _companyService.GetCompanyIdOfUser();
            var model = await _jobService.GetAllJobsOfCompanyAsync(companyId);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _jobService.GetJobCreateViewModelAsync(19);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _jobService.GetJobCreateViewModelAsync(19);
                return View(model);
            }

            var result = await _jobService.CreateJob(19, model);
            if (!result)
            {
                model = await _jobService.GetJobCreateViewModelAsync(19);
                return View(model);
            }

            return RedirectToAction("CompanyDashboard", "Company");
        }
    }
}
