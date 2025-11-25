using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CityTranslationManager : CrudManager<CityTranslation, CityTranslationViewModel, CityTranslationCreateViewModel, CityTranslationUpdateViewModel>
, ICityTranslationService
    {
        public CityTranslationManager(IRepositoryAsync<CityTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
