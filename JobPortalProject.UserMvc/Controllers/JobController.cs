using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels;
using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace JobPortalProject.UserMvc.Controllers
{
    [Authorize(Roles = "Company")]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IJobListingService _jobListingService;
        private readonly ICompanyService _companyService;
        private readonly IJobExtraBenefitService _benefitService;
        private readonly IJobResponsibilityService _responsibilityService;
        private readonly IJobApplicationService _jobApplicationService;

        public JobController(IJobService jobService, IJobListingService jobListingService, ICompanyService companyService, IJobExtraBenefitService benefitService, IJobResponsibilityService responsibilityService, IJobApplicationService jobApplicationService)
        {
            _jobService = jobService;
            _jobListingService = jobListingService;
            _companyService = companyService;
            _benefitService = benefitService;
            _responsibilityService = responsibilityService;
            _jobApplicationService = jobApplicationService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(JobFilterViewModel filter)
        {
            var jobListingViewModel = await _jobListingService.GetPagedJobListingViewModel(filter);

            return View(jobListingViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ClearFilters()
        {
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            int jobId = int.Parse(id.Split('-').Last());

            var job = await _jobService.GetByIdAsync(jobId);
           
            if (job == null)
                return NotFound();

            if (await _jobApplicationService.CheckIfJobApplied(jobId))
                job.IsApplied = true;

            return View(job);
        }

        
        public async Task<IActionResult> JobList(JobFilterViewModel filter)
        {
            var companyId = await _companyService.GetCompanyIdOfUser();
            var model = await _jobService.GetPagedJobsOfCompanyModel(filter, companyId);

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

            return RedirectToAction("Dashboard", "Company");
        }

        public async Task<IActionResult> Update(string id)
        {
            int jobId = int.Parse(id.Split('-').Last());
            var model = await _jobService.GetUpdateViewModel(jobId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, JobUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await _jobService.GetUpdateViewModel(id);
                return View(model);
            }

            int companyId =await _companyService.GetCompanyIdOfUser();
            model.CompanyId = companyId;
            var result = await _jobService.UpdateAsync(id, model);
            if (result)
            {
                return RedirectToAction("JobList");
            }
            else
            {
                model = await _jobService.GetUpdateViewModel(id);
                return View(model);
            }
        }

        public async Task<IActionResult> SoftDelete(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null)
                return BadRequest();

            var deleted = await _jobService.SoftDeleteJob(id);
            if (deleted)
                return NoContent();
            else
                return RedirectToAction(nameof(JobList));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null)
                return BadRequest();

            var result = await _jobService.DeactivateJob(id);
            if (result)
                return NoContent();
            else
                return RedirectToAction(nameof(JobList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJobBenefit(int id)
        {
            var benefit = await _benefitService.GetByIdAsync(id);
            if (benefit == null)
                return BadRequest();

            var deleted = await _benefitService.DeleteAsync(id);
            if (deleted)
            {
                var model = await _jobService.GetUpdateViewModel(benefit.JobId);
                var benefitHtml = await RenderPartialViewToString("_BenefitsUpdatePartial", model);

                return Json(new
                {
                    success = true,
                    benefitHtml
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJobResponsibility(int id)
        {
            var responsibility = await _responsibilityService.GetByIdAsync(id);
            if (responsibility == null)
                return BadRequest();

            var deleted = await _responsibilityService.DeleteAsync(id);
            if (deleted)
            {
                var model = await _jobService.GetUpdateViewModel(responsibility.JobId);
                var responsibilityHtml = await RenderPartialViewToString("_ResponsibilitiesUpdatePartial", model);

                return Json(new
                {
                    success = true,
                    responsibilityHtml
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost] 
        public async Task<IActionResult> AddResponsibility(JobResponsibilityCreateViewModel model)
        {
            if (model.JobId == 0)
            {
                return BadRequest();
            }

            var jobUpdateModel = await _jobService.GetUpdateViewModel(model.JobId);

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction("Update", jobUpdateModel);
            }

            var created = await _responsibilityService.CreateAsync(model);
            if(created == null)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction("Update", jobUpdateModel);
            }

            TempData["Success"] = "Responsibility added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction("Update", jobUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBenefit(JobExtraBenefitCreateViewModel model)
        {
            if (model.JobId == 0)
            {
                return BadRequest();
            }

            var jobUpdateModel = await _jobService.GetUpdateViewModel(model.JobId);

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction("Update", jobUpdateModel);
            }

            var created = await _benefitService.CreateAsync(model);
            if(created == null)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction("Update", jobUpdateModel);
            }

            TempData["Success"] = "Extra Benefit added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction("Update", jobUpdateModel);
        }

        public async Task<IActionResult> Applicants(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null)
                return NotFound();

            var model = await _jobApplicationService.GetApplicantsViewModel(id);
            model.Job = job;

            return View(model);
        }

        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using var writer = new StringWriter();

            var viewEngine = HttpContext.RequestServices.GetService<ICompositeViewEngine>();
            var viewResult = viewEngine.FindView(ControllerContext, viewName, false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Could not find view '{viewName}'");
            }

            var viewContext = new ViewContext(
                ControllerContext,  // This provides the ViewContext data
                viewResult.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return writer.ToString();
        }
    }
}
