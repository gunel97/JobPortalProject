using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CountryTranslationManager : CrudManager<CountryTranslation, CountryTranslationViewModel, CountryTranslationCreateViewModel, CountryTranslationUpdateViewModel>
, ICountryTranslationService
    {
        public CountryTranslationManager(IRepositoryAsync<CountryTranslation> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
