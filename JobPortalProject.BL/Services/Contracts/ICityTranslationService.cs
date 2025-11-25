using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICityTranslationService : ICrudService<CityTranslation, CityTranslationViewModel, CityTranslationCreateViewModel, CityTranslationUpdateViewModel>
    {
    }
}
