using AspNetCoreGeneratedDocument;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ProfileViewModels;
using JobPortalProject.DA.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace JobPortalProject.UserMvc.Controllers
{
    public class CandidateController : Controller
    { 
        private readonly ICandidateService _candidateService;
        private readonly IPersonalInfoService _personalInfoService;
        private readonly IEducationService _educationService;
        private readonly IExperienceService _experienceService;
        private readonly ICookieService _cookieService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IResumeService _resumeService;

        public CandidateController(ICandidateService candidateService, IPersonalInfoService personalInfoService, IEducationService educationService, IExperienceService experienceService, ICookieService cookieService, IJobApplicationService jobApplicationService, IResumeService resumeService)
        {
            _candidateService = candidateService;
            _personalInfoService = personalInfoService;
            _educationService = educationService;
            _experienceService = experienceService;
            _cookieService = cookieService;
            _jobApplicationService = jobApplicationService;
            _resumeService = resumeService;
        }

        public async Task<IActionResult> Dashboard()
        {

            var model = await _candidateService.GetDashboardViewModel();
            var candidate = await _candidateService.GetCandidate();
            var applications = await _jobApplicationService.GetAppliedJobModelsOfCandidate(model.CandidateId);
            model.Applications = applications.OrderByDescending(x=>x.AppliedAt).Take(5).ToList();

            return View(model);
        }

        public IActionResult Settings()
        {
          return View();
        }

        public async Task<IActionResult> PersonalInfo(int languageId)
        {
            var model = await _personalInfoService.GetPersonalInfoCreateViewModel(languageId);

            if (model == null)
                return NotFound();

            if (model.ResumeId == 0)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(ProfileTranslation), new { languageId = model.LanguageId });
            }

           
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(PersonalInfoCreateViewModel model) 
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null)
                return BadRequest();
            var resume = await _resumeService.CreateResume(candidate.Id);
            var dashboardModel = await _candidateService.GetDashboardViewModel();
            model.DashboardModel = dashboardModel;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _personalInfoService.CreatePersonalInfo(model, resume.Id);

            if (result)
            {
                return RedirectToAction(nameof(Profile), new { languageId = model.LanguageId });
            }

            return View(model);
        }

        public async Task<IActionResult> PersonalInfoUpdate()
        {
            var language = await _cookieService.GetLanguageAsync();
            var model = await _personalInfoService.GetPersonalInfoUpdateViewModel(language.Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfoUpdate(PersonalInfoUpdateViewModel updateModel)
        {
            var model = await _personalInfoService.GetPersonalInfoUpdateViewModel(updateModel.LanguageId);
            if (!ModelState.IsValid)
            {                
                return View(model);
            }
            var result =await _personalInfoService.UpdatePersonalInfo(updateModel);
            model = await _personalInfoService.GetPersonalInfoUpdateViewModel(updateModel.LanguageId);
            return View(model);
        }

        public async Task<IActionResult> ProfileUpdate()
        {
            var model = await _candidateService.GetDashboardViewModel();

            return View(model);
        }

        public async Task<IActionResult> ProfileTranslationUpdate(int id)
        {
            var model = await _personalInfoService.GetProfileUpdateViewModel();
            var translationModel = model.ProfileTranslations.FirstOrDefault(x => x.LanguageId == id);

            if (model == null || translationModel == null)
                return NotFound();

            return PartialView("_ProfileTranslationUpdatePartial", translationModel);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileTranslationUpdate(ProfileTranslationUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _personalInfoService.UpdateProfileTranslation(model);

            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> ProfileTranslation(int languageId)
        {
            var model = await _personalInfoService.GetProfileTranslationCreateViewModel(languageId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileTranslation(ProfileTranslationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _personalInfoService.CreateProfileTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(EducationTranslation), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        public async Task< IActionResult> Profile(int languageId)
        {
            var model =  await _personalInfoService.GetProfileCreateViewModel(languageId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile( ProfileCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _personalInfoService.CreateProfile(model.LanguageId, model);

            if(result)
                return RedirectToAction(nameof(Education), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        public async Task<IActionResult> EducationTranslation(int languageId)
        {
            var model = await _educationService.GetEducationTranslationPageCreateViewModel(languageId);
         
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EducationTranslation(EducationPageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _educationService.CreateEducationTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(ExperienceTranslation), new { languageId = model.LanguageId });

            return View(model);
        }

        public async Task<IActionResult> ExperienceTranslation(int languageId)
        {
            var model = await _experienceService.GetExperienceTranslationPageCreateViewModel(languageId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExperienceTranslation(ExperiencePageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _experienceService.CreateExperienceTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Dashboard));
            return View(model);
        }

        public async Task<IActionResult> Education(int languageId)
        {
            var model = await _educationService.GetEducationPageCreateViewModel(languageId);
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Education(EducationPageCreateViewModel model)
        {
            var dashboard = await _candidateService.GetDashboardViewModel();
            model.DashboardModel = dashboard;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _educationService.CreateEducation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Experience), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEducationToResume(EducationAddViewModel model)
        {
            var educationUpdateModel = await _educationService.GetEducationUpdateViewModel();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction(nameof(EducationUpdate), educationUpdateModel);
            }

            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return NotFound();
  
            var result = await _educationService.AddEducationToResume(model, candidate.Resume.Id);

            if (!result)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction(nameof(EducationUpdate), educationUpdateModel);
            }

            educationUpdateModel = await _educationService.GetEducationUpdateViewModel();
            TempData["Success"] = "Added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction(nameof(EducationUpdate), educationUpdateModel);
        }

        public async Task<IActionResult> EducationUpdate()
        {
            var model = await _educationService.GetEducationUpdateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EducationUpdate(EducationUpdatePageViewModel model)
        {
            foreach (var item in model.UpdateModels)
            {
                if (!ModelState.IsValid)
                    return View(model);
            }

            foreach (var education in model.UpdateModels)
            {
              var result =  await _educationService.UpdateAsync(education.Id, education);
            }

            model = await _educationService.GetEducationUpdateViewModel();
            return View(model);
        }

        public async Task<IActionResult> Experience(int languageId)
        {
            var model = await _experienceService.GetExperiencePageCreateViewModel(languageId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Experience(ExperiencePageCreateViewModel model)
        {
            var dashboard = await _candidateService.GetDashboardViewModel();
            model.DashboardViewModel = dashboard;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _experienceService.CreateExperience(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Dashboard));
            else
                return View(model);

        }

        public async Task<IActionResult> ExperienceUpdate()
        {
            var model = await _experienceService.GetExperienceUpdateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExperienceUpdate(ExperienceUpdatePageViewModel model)
        {
            var dashboardModel = await _candidateService.GetDashboardViewModel();
            model.Dashboard = dashboardModel;

            foreach (var item in model.Models)
            {
                if (!ModelState.IsValid)
                    return View(model);
            }

            foreach (var experience in model.Models)
            {
                var result = await _experienceService.UpdateAsync(experience.Id, experience);
            }

            model = await _experienceService.GetExperienceUpdateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddExperienceToResume(ExperienceAddViewModel model)
        {
            var experienceUpdateModel = await _experienceService.GetExperienceUpdateViewModel();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields";
                return RedirectToAction(nameof(ExperienceUpdate), experienceUpdateModel);
            }

            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return NotFound();

            var result = await _experienceService.AddExperienceToResume(model, candidate.Resume.Id);

            if (!result)
            {
                TempData["Error"] = "Cant create";
                return RedirectToAction(nameof(ExperienceUpdate), experienceUpdateModel);
            }

            experienceUpdateModel = await _experienceService.GetExperienceUpdateViewModel();
            TempData["Success"] = "Added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction(nameof(ExperienceUpdate), experienceUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null)
                return BadRequest();

            var deleted = await _educationService.DeleteAsync(id);
            if (deleted)
            {
                var model = await _educationService.GetEducationUpdateViewModel();
                var educationHtml = await RenderPartialViewToString("_EducationUpdatePartial", model);

                return Json(new
                {
                    success = true,
                    educationHtml
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExperience(int id)
        {
            var experience = await _experienceService.GetByIdAsync(id);
            if (experience == null)
                return BadRequest();

            var deleted = await _experienceService.DeleteAsync(id);
            if (deleted)
            {
                var model = await _experienceService.GetExperienceUpdateViewModel();
                var experienceHtml = await RenderPartialViewToString("_ExperienceUpdatePartial", model.Models);

                return Json(new
                {
                    success = true,
                    experienceHtml
                });
            }
            else
            {
                return BadRequest();
            }
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

        public async Task<IActionResult> AppliedJobs(JobApplicationsOfCandidateFilterViewModel filter)
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null)
                return BadRequest();
            var model = await _jobApplicationService.GetAppliedJobsPageOfCandidateViewModel(filter,candidate.Id);
            return View(model);
        }

        public async Task<IActionResult> Resume()
        {
            var candidate = await _candidateService.GetCandidate();
            if (candidate == null || candidate.Resume == null)
                return BadRequest();

            var resumeModel = await _resumeService.GetResumeBase(candidate.Resume.Id);
            resumeModel.PersonalInfo = await _personalInfoService.GetPersonalInfoViewModel(resumeModel.Id);
            resumeModel.Educations = await _educationService.GetEducationModelsOfResume(resumeModel.Id);
            resumeModel.Experiences = await _experienceService.GetExperienceModelsOfResume(resumeModel.Id);

            return View(resumeModel);
        }
    }
}
