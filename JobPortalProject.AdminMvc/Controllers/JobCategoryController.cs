using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryIndexService _jobCategoryIndexService;
        private readonly IJobCategoryService _jobCategoryService;

        public JobCategoryController(IJobCategoryIndexService jobCategoryIndexService, IJobCategoryService jobCategoryService)
        {
            _jobCategoryIndexService = jobCategoryIndexService;
            _jobCategoryService = jobCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _jobCategoryIndexService.GetJobCategoryIndexModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobCategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

           var result =  await _jobCategoryService.CreateJobCategoryAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
        //    int categoryId = int.Parse(id.Split('-').Last());
            var updateModel =await  _jobCategoryService.GetUpdateViewModel(id);

            return View(updateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(JobCategoryUpdateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model = await _jobCategoryService.GetUpdateViewModel(model.Id);
                return View(model);
            }

            var result = await _jobCategoryService.UpdateJobCategoryAsync(model);
            if (result)
                return RedirectToAction(nameof(Index));
            else
            {
                model = await _jobCategoryService.GetUpdateViewModel(model.Id);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _jobCategoryService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
