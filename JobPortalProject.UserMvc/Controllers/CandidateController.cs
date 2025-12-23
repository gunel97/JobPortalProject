using AspNetCoreGeneratedDocument;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Mvc;
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
        public CandidateController(ICandidateService candidateService, IPersonalInfoService personalInfoService, IEducationService educationService, IExperienceService experienceService)
        {
            _candidateService = candidateService;
            _personalInfoService = personalInfoService;
            _educationService = educationService;
            _experienceService = experienceService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _candidateService.GetDashboardViewModel();

            return View(model);
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _personalInfoService.CreatePersonalInfo(model);

            if (result)
            {
                return RedirectToAction(nameof(Profile), new { languageId = model.LanguageId });
            }

            return View(model);
        }

        public async Task<IActionResult> PersonalInfoUpdate(int languageId)
        {
            var model = await _personalInfoService.GetPersonalInfoUpdateViewModel(languageId);

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

        [HttpPost]
        public async Task<IActionResult> AddEducationToResume(EducationAddViewModel model)
        {
            var educationUpdateModel = await _personalInfoService.GetEducationUpdateViewModel();
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

            educationUpdateModel = await _personalInfoService.GetEducationUpdateViewModel();
            TempData["Success"] = "Added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction(nameof(EducationUpdate), educationUpdateModel);
        }

        public async Task<IActionResult> EducationUpdate()
        {
            var model = await _personalInfoService.GetEducationUpdateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EducationUpdate(List<EducationUpdateViewModel> model)
        {
            foreach (var item in model)
            {
                if (!ModelState.IsValid)
                    return View(model);
            }

            foreach (var education in model)
            {
              var result =  await _educationService.UpdateAsync(education.Id, education);
            }

            model = await _personalInfoService.GetEducationUpdateViewModel();
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

        public async Task<IActionResult> ExperienceUpdate()
        {
            var model = await _personalInfoService.GetExperienceUpdateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExperienceUpdate(List<ExperienceUpdateViewModel> model)
        {
            var dashboardModel = await _candidateService.GetDashboardViewModel();
            
            foreach (var item in model)
            {
                item.Dashboard=dashboardModel;
                if (!ModelState.IsValid)
                    return View(model);
            }

            foreach (var experience in model)
            {
                var result = await _experienceService.UpdateAsync(experience.Id, experience);
            }

            model = await _personalInfoService.GetExperienceUpdateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddExperienceToResume(ExperienceAddViewModel model)
        {
            var experienceUpdateModel = await _personalInfoService.GetExperienceUpdateViewModel();
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

            experienceUpdateModel = await _personalInfoService.GetExperienceUpdateViewModel();
            TempData["Success"] = "Added successfully!";
            TempData["CloseModal"] = "true";
            return RedirectToAction(nameof(ExperienceUpdate), experienceUpdateModel);
        }
        
    }
}
