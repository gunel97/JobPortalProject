using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace JobPortalProject.BL.Services.Implementations
{
    public class WorkingFieldManager : CrudManager<WorkingField, WorkingFieldViewModel, WorkingFieldCreateViewModel, WorkingFieldUpdateViewModel>
, IWorkingFieldService
    {
        private readonly IWorkingFieldTranslationService _workingFieldTranslationService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;
        public WorkingFieldManager(IRepositoryAsync<WorkingField> repository, IMapper mapper, IWorkingFieldTranslationService workingFieldTranslationService, ICloudinaryService cloudinaryService, FileService fileService) : base(repository, mapper)
        {
            _workingFieldTranslationService = workingFieldTranslationService;
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
        }

        public async Task<bool> CreateWorkingField(int companyId, WorkingFieldCreateViewModel model)
        {
            model.CompanyId = companyId;
            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var result = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.WorkingFieldImagePath);

                if (result.Success)
                {
                    model.IconUrl = result.Url;
                    model.IconPublicId = result.PublicId;
                }
            }
            var workingField = await CreateAsync(model);

            if (workingField == null)
                return false;

            else
            {
                foreach (var translationModel in model.WorkingFieldTranslationCreateViewModels)
                {
                    var workingFieldTranslationCreateModel = new WorkingFieldTranslationCreateViewModel
                    {
                        WorkingFieldId = workingField.Id,
                        LanguageId = translationModel.LanguageId,
                        Name = translationModel.Name,
                        Description = translationModel.Description,
                    };

                    var workingFieldTranslation = await _workingFieldTranslationService.CreateAsync(workingFieldTranslationCreateModel);
                    if (workingFieldTranslation == null)
                    {
                        await DeleteAsync(workingField.Id);
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<List<WorkingFieldUpdateViewModel>> GetUpdateViewModelAsync(int companyId, int selectedLanguageId)
        {
            var workingFields = await Repository.GetAllAsync(predicate: x => x.CompanyId == companyId,
                include: x => x.Include(w => w.Translations.Where(t => t.LanguageId == selectedLanguageId)!));

            if (workingFields == null)
                return null!;
            var workingFieldUpdateViewModels = new List<WorkingFieldUpdateViewModel>();

            foreach(var workingField in workingFields)
            {
                if (workingField.Translations.Any())
                {
                    workingFieldUpdateViewModels.Add(new WorkingFieldUpdateViewModel
                    {
                        WorkingFieldId = workingField.Id,
                        CompanyId = workingField.CompanyId,
                        IconUrl = workingField.IconUrl,
                        Name = workingField.Translations.FirstOrDefault(x => x.LanguageId == selectedLanguageId).Name,
                        Description=workingField.Translations.FirstOrDefault(x=>x.LanguageId==selectedLanguageId).Description,
                        WorkingFieldTranslationId=workingField.Translations.FirstOrDefault(x=>x.LanguageId==selectedLanguageId).Id
                    });
                }
            }

            return workingFieldUpdateViewModels;
        }

        public async Task<List<SelectListItem>> GetWorkingFieldSelectListItemsAsync(int companyId, int selectedLanguageId)
        {
            var workingFields =  await Repository.GetAllAsync(predicate: x =>
                x.CompanyId == companyId && !x.IsDeleted, include: x=>x.Include(t=>t.Translations));

            var workingFieldSelectListItems = new List<SelectListItem>();
            foreach (var workingField in workingFields)
            {
                var translation = workingField.Translations.FirstOrDefault(x => x.LanguageId == selectedLanguageId);
                if (workingField.Translations.Any() && translation==null )
                {
                    workingFieldSelectListItems.Add(
                        new SelectListItem(workingField.Translations.FirstOrDefault()!.Name, workingField.Id.ToString()));
                }
            }

            return workingFieldSelectListItems;
        }

        public async Task<WorkingFieldTranslationViewModel> CreateWorkingFieldTranslationAsync(AddWorkingFieldTranslationViewModel model)
        {
            if (model.TranslationCreateViewModel == null)
                return null!;

            model.TranslationCreateViewModel.WorkingFieldId = model.WorkingFieldId;
            model.TranslationCreateViewModel.LanguageId = model.SelectedLanguageId;

            var isCreated = await _workingFieldTranslationService.CreateAsync(model.TranslationCreateViewModel);
            return isCreated;
        }

        public async Task<bool> UpdateWorkingFieldAsync(int languageId, int workingFieldId, WorkingFieldUpdateViewModel model)
        {
            var workingField = await Repository.GetAsync(predicate: x => x.Id == workingFieldId,
                    include: x => x.Include(x => x.Translations.Where(t => t.LanguageId == languageId)));

            if (workingField == null)
                return false;

            var translation = workingField.Translations.FirstOrDefault(x => x.LanguageId == languageId);
            if (translation == null) 
                return false;

            var translationUpdateModel = new WorkingFieldTranslationUpdateViewModel
            {
                Id = translation.Id,
                LanguageId = languageId,
                Name = model.Name,
                Description = model.Description,
                WorkingFieldId = workingFieldId
            };

            await _workingFieldTranslationService.UpdateAsync(translation.Id, translationUpdateModel);

            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var resultLogo = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.WorkingFieldImagePath);

                if (resultLogo.Success)
                {
                    if (workingField.IconPublicId != null)
                    {
                        var deleteResult = await _cloudinaryService.DeleteImageAsync(workingField.IconPublicId);
                    }
                    model.IconUrl = resultLogo.Url;
                    model.IconPublicId = resultLogo.PublicId;
                }
            }

            model.WorkingFieldId = workingFieldId;
            await Repository.UpdateAsync(workingField);

            return true;


            //workingFieldModel.IconUrl = workingField.IconUrl;
            //workingFieldModel.IconPublicId = workingField.IconPublicId;
            //workingFieldModel.CompanyId = existedCompany.Id;

            //if (workingFieldModel.IconFile != null)
            //{
            //    if (!_fileService.IsImageFile(workingFieldModel.IconFile))
            //        throw new ArgumentException("The file is not a valid image.", nameof(workingFieldModel.IconFile));

            //    var resultLogo = await _cloudinaryService.UploadImageAsync(workingFieldModel.IconFile, FilePathConstants.WorkingFieldImagePath);

            //    if (resultLogo.Success)
            //    {
            //        if (workingField.IconPublicId != null)
            //        {
            //            var deleteResult = await _cloudinaryService.DeleteImageAsync(workingField.IconPublicId);
            //        }
            //        workingFieldModel.IconUrl = resultLogo.Url;
            //        workingFieldModel.IconPublicId = resultLogo.PublicId;
            //    }
            //}

            //var workingFieldTranslationModel = workingFieldModel.WorkingFieldTranslationUpdateViewModel;
            //if (workingFieldTranslationModel == null)
            //    return false;

            //workingFieldTranslationModel.WorkingFieldId = workingFieldModel.WorkingFieldId;
            //workingFieldTranslationModel.LanguageId = selectedLanguageId;
            //await _workingFieldTranslationService.UpdateAsync(workingFieldTranslationModel.WorkingFieldTranslationId, workingFieldTranslationModel);


        }

        public async Task<bool> AddTranslationToExistingWorkingField(WorkingFieldTranslationCreateViewModel model)
        {
            var workingField = await Repository.GetAsync(predicate: x => x.Id == model.WorkingFieldId,
                include: x => x.Include(t => t.Translations));

            if (workingField == null)
                return false;

            workingField.Translations.Add(new WorkingFieldTranslation
            {
                WorkingFieldId = model.WorkingFieldId,
                LanguageId = model.LanguageId,
                Name = model.Name,
                Description = model.Description,
            });

            var result =  await Repository.UpdateAsync(workingField);
            if (result == null)
                return false;

            return true;    
        }
    }


}
