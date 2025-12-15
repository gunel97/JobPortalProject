using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ISocialMediaService : ICrudService<SocialMedia, SocialMediaViewModel, SocialMediaCreateViewModel, SocialMediaUpdateViewModel>
    {
        public Task<List<SelectListItem>> GetSocialMediaListItems();
    }
  
}
