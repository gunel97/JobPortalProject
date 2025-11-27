using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.SocialMediaViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class SocialMediaManager : CrudManager<SocialMedia, SocialMediaViewModel, SocialMediaCreateViewModel, SocialMediaUpdateViewModel>
, ISocialMediaService
    {
        public SocialMediaManager(IRepositoryAsync<SocialMedia> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
