using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class LanguageManager:CrudManager<Language, LanguageViewModel, LanguageCreateViewModel, LanguageUpdateViewModel>
        ,ILanguageService
    {
        private readonly FileService _fileService;
        private readonly ICloudinaryService _cloudinaryService;

        public LanguageManager(IRepositoryAsync<Language> repository, IMapper mapper, FileService fileService, ICloudinaryService cloudinaryService) : base(repository, mapper)
        {
            _fileService = fileService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<LanguageUpdateViewModel> GetUpdateViewModel(int id)
        {
            var language = await Repository.GetByIdAsync(id);
            var model = Mapper.Map<LanguageUpdateViewModel>(language);

            return model;
        }

        public override async Task<bool> UpdateAsync(int id, LanguageUpdateViewModel model)
        {
            var language = await Repository.GetByIdAsync(id);
            if (language == null)
                return false;

            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var result = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.IconImagePath);
                if (result.Success)
                {
                    model.IconPublicId = result.PublicId;
                    model.IconUrl=result.Url;
                    await _cloudinaryService.DeleteImageAsync(language.IconPublicId);
                }
            }

            return await base.UpdateAsync(id, model);
        }
        public override async Task<LanguageViewModel> CreateAsync(LanguageCreateViewModel model)
        {
            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var result = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.IconImagePath);

                if (result.Success)
                {
                    model.IconUrl = result.Url;
                    model.IconPublicId = result.PublicId;
                }
            }

            return await base.CreateAsync(model);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var language = await Repository.GetByIdAsync(id);
            if (language == null)
                return false;

            var result = await base.DeleteAsync(id);
            if (!result)
                return false;

            await _cloudinaryService.DeleteImageAsync(language.IconPublicId);
            return true;

        }


    }
}
