using AutoMapper;
using JobPortalProject.BL.Constants;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.BioViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class BioManager : CrudManager<Bio, BioViewModel, BioCreateViewModel, BioUpdateViewModel>
, IBioService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly FileService _fileService;

        public BioManager(IRepositoryAsync<Bio> repository, IMapper mapper, ICloudinaryService cloudinaryService, FileService fileService) : base(repository, mapper)
        {
            _cloudinaryService = cloudinaryService;
            _fileService = fileService;
        }

        public override async Task<BioViewModel> CreateAsync(BioCreateViewModel model)
        {
            if (model.LogoFile != null)
            {
                if (!_fileService.IsImageFile(model.LogoFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.LogoFile));

                var result = await _cloudinaryService.UploadImageAsync(model.LogoFile, FilePathConstants.IconImagePath);

                if (result.Success)
                {
                    model.LogoUrl = result.Url;
                    model.LogoPublicId = result.PublicId;
                }
            }
            return await base.CreateAsync(model);
        }

        public override async Task<bool> UpdateAsync(int id, BioUpdateViewModel model)
        {
            var bio = await Repository.GetByIdAsync(id);
            if (bio == null)
                return false;

            if (model.LogoFile != null)
            {
                if (!_fileService.IsImageFile(model.LogoFile))
                    throw new ArgumentException("The file is not a valid image.", nameof(model.LogoFile));

                var result = await _cloudinaryService.UploadImageAsync(model.LogoFile, FilePathConstants.LogoPath);
                if (result.Success)
                {
                    model.LogoPublicId = result.PublicId;
                    model.LogoUrl = result.Url;
                    await _cloudinaryService.DeleteImageAsync(bio.LogoPublicId);
                }
            }
            else
            {
                model.LogoPublicId = bio.LogoPublicId;
                model.LogoUrl = bio.LogoUrl;
            }

            return await base.UpdateAsync(id, model);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var bio = await Repository.GetByIdAsync(id);
            if (bio == null)
                return false;

            var result = await base.DeleteAsync(id);
            if (!result)
                return false;

            await _cloudinaryService.DeleteImageAsync(bio.LogoPublicId);
            return true;
        }

        public async Task<BioUpdateViewModel> GetUpdateViewModel()
        {
            var bios = await Repository.GetAllAsync();
            var bio = bios.FirstOrDefault();
            if (bio == null)
                return null!;

            var model = Mapper.Map<BioUpdateViewModel>(bio);
            return model;
        }

    }


}
