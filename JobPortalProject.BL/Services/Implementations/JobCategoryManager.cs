using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class JobCategoryManager : CrudManager<JobCategory, JobCategoryViewModel, JobCategoryCreateViewModel, JobCategoryUpdateViewModel>
    , IJobCategoryService
    {
        public JobCategoryManager(IRepositoryAsync<JobCategory> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<List<SelectListItem>> GetJobCategorySelectListItems(int selectedLanguageId)
        {
            var jobCategoryListItems = new List<SelectListItem>();

            var jobCategories = await Repository.GetAllAsync(include:
                x => x.Include(x => x.JobCategoryTranslations.Where(t => t.LanguageId == selectedLanguageId)));
            var jobCategoryViewModelsList = jobCategories.Select(x => Mapper.Map<JobCategoryViewModel>(x)).ToList();

            jobCategoryViewModelsList.ForEach(x => jobCategoryListItems.Add(
                new SelectListItem(x.Name, x.Id.ToString())));

            return jobCategoryListItems;
        }
    }


}
