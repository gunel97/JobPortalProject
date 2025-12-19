using JobPortalProject.BL.Services.Contracts;
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

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CreateResume()
        {
            var model = _candidateService.GetPersonalInfoCreateViewModel();

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateResume(PersonalInfoCreateViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _candidateService.CreatePersonalInfo(model);

            if (result)
            {
                return RedirectToAction(nameof(Profile));
            }

            return View(model);
        }

        public async Task< IActionResult> Profile()
        {
            var model =  await _candidateService.GetProfileCreateViewModel(1);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile( ProfileCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _candidateService.CreateProfile(1, model);

            if(result)
                return RedirectToAction(nameof(Dashboard));
            else
                return View(model);
        }
    }
}
