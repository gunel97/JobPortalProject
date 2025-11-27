using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ISocialMediaService : ICrudService<SocialMedia, SocialMediaViewModel, SocialMediaCreateViewModel, SocialMediaUpdateViewModel>
    {
    }
}
