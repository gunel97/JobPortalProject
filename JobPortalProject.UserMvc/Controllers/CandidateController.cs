using AspNetCoreGeneratedDocument;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace JobPortalProject.UserMvc.Controllers
{
    public class CandidateController : Controller
    { 
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _candidateService.GetDashboardViewModel();

            return View(model);
        }

        public async Task<IActionResult> PersonalInfo(int languageId)
        {
            var model = await _candidateService.GetPersonalInfoCreateViewModel(languageId);

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _candidateService.CreatePersonalInfo(model);

            if (result)
            {
                return RedirectToAction(nameof(Profile), new { languageId = model.LanguageId });
            }

            return View(model);
        }

        public async Task<IActionResult> PersonalInfoUpdate(int languageId)
        {
            var model = await _candidateService.GetPersonalInfoUpdateViewModel(languageId);

            return View(model);
        }

        public async Task<IActionResult> ProfileTranslation(int languageId)
        {
            var model = await _candidateService.GetProfileTranslationCreateViewModel(languageId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileTranslation(ProfileTranslationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _candidateService.CreateProfileTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(EducationTranslation), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        public async Task< IActionResult> Profile(int languageId)
        {
            var model =  await _candidateService.GetProfileCreateViewModel(languageId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile( ProfileCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _candidateService.CreateProfile(model.LanguageId, model);

            if(result)
                return RedirectToAction(nameof(Education), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        public async Task<IActionResult> EducationTranslation(int languageId)
        {
            var model = await _candidateService.GetEducationTranslationPageCreateViewModel(languageId);
         
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EducationTranslation(EducationPageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _candidateService.CreateEducationTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(ExperienceTranslation), new { languageId = model.LanguageId });

            return View(model);
        }

        public async Task<IActionResult> ExperienceTranslation(int languageId)
        {
            var model = await _candidateService.GetExperienceTranslationPageCreateViewModel(languageId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExperienceTranslation(ExperiencePageCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _candidateService.CreateExperienceTranslation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Dashboard));
            return View(model);
        }

        public async Task<IActionResult> Education(int languageId)
        {
            var model = await _candidateService.GetEducationPageCreateViewModel(languageId);
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Education(EducationPageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _candidateService.CreateEducation(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Experience), new { languageId = model.LanguageId });
            else
                return View(model);
        }

        public async Task<IActionResult> Experience(int languageId)
        {
            var model = await _candidateService.GetExperiencePageCreateViewModel(languageId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Experience(ExperiencePageCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _candidateService.CreateExperience(model.LanguageId, model);
            if (result)
                return RedirectToAction(nameof(Dashboard));
            else
                return View(model);

        }
        
    }
}
