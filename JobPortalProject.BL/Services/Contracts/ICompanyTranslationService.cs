using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyTranslationService : ICrudService<CompanyTranslation, CompanyTranslationViewModel, CompanyTranslationCreateViewModel, CompanyTranslationUpdateViewModel>
    {
    }
}
