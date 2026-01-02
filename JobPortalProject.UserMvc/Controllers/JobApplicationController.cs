using AspNetCoreGeneratedDocument;
using JobPortalProject.BL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly ICandidateService _candidateService;
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationController(ICandidateService candidateService, IJobApplicationService jobApplicationService)
        {
            _candidateService = candidateService;
            _jobApplicationService = jobApplicationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null)
                return BadRequest();

            var message = "";
            var result = await _jobApplicationService.CancelJobApplication(id, candidate.Id);
            if (result)
            {
                message = "Job Application Cancelled";
                return Json(new
                {
                    success = true,
                    message
                });
            }
            else
            {
                message = "Error occurred";
                return Json(new
                {
                    success = false,
                    message
                });
            }
        }

        public async Task<IActionResult> Reject(int jobId, int candidateId)
        {
            var result = await _jobApplicationService.RejectJobApplication(jobId, candidateId);
            if (!result)
            {
                TempData["Error"] = "Error";
            }
            TempData["CloseModal"] = "true";

            return RedirectToAction(nameof(JobController.Applicants), nameof(JobController).Replace("Controller", ""), new { id = jobId });
        }

        public async Task<IActionResult> Accept(int jobId, int candidateId)
        {
            var result = await _jobApplicationService.AcceptJobApplication(jobId, candidateId);
            if (!result)
            {
                TempData["Error"] = "Error";
            }
            TempData["CloseModal"] = "true";

            return RedirectToAction(nameof(JobController.Applicants), nameof(JobController).Replace("Controller", ""), new { id = jobId });
        }

        public async Task<IActionResult> Interview(int jobId, int candidateId)
        {
            var result = await _jobApplicationService.InterviewJobApplication(jobId, candidateId);
            if (!result)
            {
                TempData["Error"] = "Error";
            }
            TempData["CloseModal"] = "true";

            return RedirectToAction(nameof(JobController.Applicants), nameof(JobController).Replace("Controller", ""), new { id = jobId });
        }

        [HttpPost]
        public async Task<IActionResult> Apply(int id)
        {
            var candidate = await _candidateService.GetCandidate();
            var message = "";
            if (candidate == null)
            {
                message = "Login/Register to create resume";
                return Json(new
                {
                    success = false,
                    message
                });
            }
            if (candidate.Resume == null)
            {
                message = "Create resume";
                return Json(new
                {
                    success = false,
                    message
                });
            }

            var result = await _jobApplicationService.ApplyJob(id, candidate.Id);

            if (result)
                return Json(new
                {
                    success = true
                });

            else
            {
                message = "An error occurred";
                return Json(new
                {
                    success = false,
                    message
                });
            }
        }
    }
}
