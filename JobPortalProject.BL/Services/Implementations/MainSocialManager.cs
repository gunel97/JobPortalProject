using AutoMapper;
using CloudinaryDotNet.Actions;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.MainSocialViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe;

namespace JobPortalProject.BL.Services.Implementations
{
    public class MainSocialManager : CrudManager<MainSocial, MainSocialViewModel, MainSocialCreateViewModel, MainSocialUpdateViewModel>
, IMainSocialService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;


        public MainSocialManager(IRepositoryAsync<MainSocial> repository, IMapper mapper, ICloudinaryService cloudinaryService, FileService fileService) : base(repository, mapper)
        {
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
        }

        public async Task<MainSocialUpdateViewModel> GetUpdateViewModel(int id)
        {
            var social = await Repository.GetByIdAsync(id);
            var model = Mapper.Map<MainSocialUpdateViewModel>(social);

            return model;
        }

        public async override Task<MainSocialViewModel> CreateAsync(MainSocialCreateViewModel model)
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

        public async override Task<bool> UpdateAsync(int id, MainSocialUpdateViewModel model)
        {
            var social = await Repository.GetByIdAsync(id);
            if (social == null)
                return false;

            if (model.IconFile != null)
            {
                if (!_fileService.IsImageFile(model.IconFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.IconFile));

                var result = await _cloudinaryService.UploadImageAsync(model.IconFile, FilePathConstants.IconImagePath);
                if (result.Success)
                {
                    model.IconPublicId = result.PublicId;
                    model.IconUrl = result.Url;
                    await _cloudinaryService.DeleteImageAsync(social.IconPublicId);
                }
            }
            else
            {
                model.IconUrl = social.IconUrl;
                model.IconPublicId = social.IconPublicId;
            }
                return await base.UpdateAsync(id, model);
        }
    }
}
