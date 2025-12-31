using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(JobCategoryFilterViewModel filter)
        {
            var model = await _jobCategoryIndexService.GetPagedJobCategoryIndexModel(filter);

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            int Id = int.Parse(id.Split('-').Last());
            var model = await _jobCategoryService.GetDetailsViewModel(Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobCategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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
            var jobCategory = await _jobCategoryService.GetAsync(predicate: x => x.Id == id, include: x => x.Include(x => x.Jobs));

            if (jobCategory.JobIds.Any())
                return BadRequest();

            var isDeleted = await _jobCategoryService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
