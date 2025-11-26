using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICompanyTypeTranslationService : ICrudService<CompanyTypeTranslation, CompanyTypeTranslationViewModel, CompanyTypeTranslationCreateViewModel, CompanyTypeTranslationUpdateViewModel>
    {
    }
}
