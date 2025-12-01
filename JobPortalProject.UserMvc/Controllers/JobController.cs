using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IJobListingService _jobListingService;

        public JobController(IJobService jobService, IJobListingService jobListingService)
        {
            _jobService = jobService;
            _jobListingService = jobListingService;
        }

        public async Task<IActionResult> Index()
        {
            var jobListingViewModel = await _jobListingService.GetJobListingViewModel();

            return View(jobListingViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            int jobId = int.Parse(id.Split('-').Last());

            var job = await _jobService.GetAsync(predicate: x=>x.Id==jobId);

            if (job == null)
                return NotFound();

            return View(job);
        }
    }
}
