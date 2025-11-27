using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanySocialService : ICrudService<CompanySocial, CompanySocialViewModel, CompanySocialCreateViewModel, CompanySocialUpdateViewModel>
    {
    }
}
