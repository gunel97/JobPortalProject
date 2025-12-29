using AutoMapper;
using JobPortalProject.BL.Constants;
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
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;
        private readonly ILanguageService _languageService;
        private readonly IJobCategoryTranslationService _jobCategoryTranslationService;

        public JobCategoryManager(IRepositoryAsync<JobCategory> repository, IMapper mapper, ICloudinaryService cloudinaryService, FileService fileService, IJobCategoryTranslationService jobCategoryTranslationService, ILanguageService languageService) : base(repository, mapper)
        {
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
            _jobCategoryTranslationService = jobCategoryTranslationService;
            _languageService = languageService;
        }

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

        public async Task<JobCategoryViewModel> CreateJobCategoryAsync(JobCategoryCreateViewModel model)
        {
            if(model.ImageFile != null)
            {
                if (!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));

                var result = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.JobCategoryImagePath);

                if (result.Success)
                {
                    model.ImageUrl = result.Url;
                    model.ImagePublicId = result.PublicId;
                }
            }

            var category = await CreateAsync(model);
            if (category != null)
            {
                foreach (var translation in model.Translations)
                {
                    var translationModel = new JobCategoryTranslationCreateViewModel
                    {
                        LanguageId = translation.LanguageId,
                        Name = translation.Name,
                        JobCategoryId = category.Id
                    };
                    var translationResult = await _jobCategoryTranslationService.CreateAsync(translationModel);
                    if (translationResult == null)
                        await DeleteAsync(category.Id);
                }
            }

            return category!;
        }

        public async Task<bool> UpdateJobCategoryAsync(JobCategoryUpdateViewModel model)
        {
            var category = await Repository.GetAsync(predicate: x=>x.Id== model.Id,
                include: x=>x.Include(t=>t.JobCategoryTranslations));
            if (category == null)
                return false;

            if(model.ImageFile != null)
            {
                if(!_fileService.IsImageFile(model.ImageFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.ImageFile));

                var result = await _cloudinaryService.UploadImageAsync(model.ImageFile, FilePathConstants.JobCategoryImagePath);
                if (result.Success)
                {
                    if (category.ImagePublicId != null)
                    {
                        await _cloudinaryService.DeleteImageAsync(category.ImagePublicId);
                    }
                    category.ImagePublicId = result.PublicId;
                    category.ImageUrl = result.Url;
                }
            }
            category.IsDeleted = model.IsDeleted;

            foreach(var translation in model.Translations)
            {
                translation.JobCategoryId = category.Id;
                await _jobCategoryTranslationService.UpdateAsync(translation.Id, translation);
            }

            await Repository.UpdateAsync(category);
            return true;
        }

        public async Task<JobCategoryUpdateViewModel> GetUpdateViewModel(int id)
        {
            var category = await Repository.GetAsync(predicate: x => x.Id == id,
                include: x => x.Include(x => x.JobCategoryTranslations));
            if (category == null)
                return null!;
            var languages = await _languageService.GetAllAsync();

            var model = new JobCategoryUpdateViewModel
            {
                Id = category.Id,
                ImageUrl = category.ImageUrl,
                IsDeleted= category.IsDeleted,
                Translations = category.JobCategoryTranslations.Select(x => new JobCategoryTranslationUpdateViewModel
                {
                    Id = x.Id,
                    JobCategoryId = category.Id,
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    LanguageIcon=languages.FirstOrDefault(t=>t.Id==x.LanguageId)!.IconUrl,
                }).ToList()
            };

            return model;
        }
    }
}
