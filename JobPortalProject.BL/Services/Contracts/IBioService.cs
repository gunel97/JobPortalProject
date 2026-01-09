using JobPortalProject.BL.ViewModels.BioViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IBioService : ICrudService<Bio, BioViewModel, BioCreateViewModel, BioUpdateViewModel>
    {
        public Task<BioUpdateViewModel> GetUpdateViewModel();
    }
}
