using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.BioViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class BioController : Controller
    {
        private readonly IBioService _bioService;

        public BioController(IBioService bioService)
        {
            _bioService = bioService;
        }

        public async Task<IActionResult> Index()
        {
            var bios = await _bioService.GetAllAsync();
            var model = bios.FirstOrDefault();
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var result = await _bioService.CreateAsync(model);

            if (result == null)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var bio = await _bioService.GetAllAsync();
            var model = bio.FirstOrDefault();
            if (model == null)
                return NotFound();
            var isDeleted = await _bioService.DeleteAsync(model.Id);
            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update()
        {
            var model = await _bioService.GetUpdateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BioUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _bioService.UpdateAsync(model.Id, model);
            if (!result)          
              return View(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
