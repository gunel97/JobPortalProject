using JobPortalProject.BL.ViewModels.MainSocialViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IMainSocialService : ICrudService<MainSocial, MainSocialViewModel, MainSocialCreateViewModel, MainSocialUpdateViewModel>
    {
        public Task<MainSocialUpdateViewModel> GetUpdateViewModel(int id);
    }
}
