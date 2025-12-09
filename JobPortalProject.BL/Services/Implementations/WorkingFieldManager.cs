using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortalProject.BL.Services.Implementations
{
    public class WorkingFieldManager : CrudManager<WorkingField, WorkingFieldViewModel, WorkingFieldCreateViewModel, WorkingFieldUpdateViewModel>
, IWorkingFieldService
    {
        private readonly IWorkingFieldTranslationService _workingFieldTranslationService;
        public WorkingFieldManager(IRepositoryAsync<WorkingField> repository, IMapper mapper, IWorkingFieldTranslationService workingFieldTranslationService) : base(repository, mapper)
        {
            _workingFieldTranslationService = workingFieldTranslationService;
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
                        CompanyId = companyId,
                        IconUrl = workingField.IconUrl,
                        WorkingFieldTranslationUpdateViewModel = new WorkingFieldTranslationUpdateViewModel
                        {
                            WorkingFieldTranslationId = workingField.Translations.FirstOrDefault()!.Id,
                            WorkingFieldId = workingField.Id,
                            LanguageId = selectedLanguageId,
                            Name = workingField.Translations.FirstOrDefault()!.Name,
                            Description = workingField.Translations.FirstOrDefault()!.Description,
                        }
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
    }


}
