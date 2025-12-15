using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class SocialMediaManager : CrudManager<SocialMedia, SocialMediaViewModel, SocialMediaCreateViewModel, SocialMediaUpdateViewModel>
, ISocialMediaService
    {
        public SocialMediaManager(IRepositoryAsync<SocialMedia> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<List<SelectListItem>> GetSocialMediaListItems()
        {
            var socialSelectListItems = new List<SelectListItem>();
            var socials = await Repository.GetAllAsync();
            var socialViewModels = socials.Select(
                x => Mapper.Map<SocialMediaViewModel>(x)).ToList();

            socialViewModels.ForEach(x => socialSelectListItems.Add(
                new SelectListItem(x.Title, x.Id.ToString())));

            return socialSelectListItems;
        }
    }


}
