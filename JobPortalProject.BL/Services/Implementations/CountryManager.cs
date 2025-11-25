using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CountryManager : CrudManager<Country, CountryViewModel, CountryCreateViewModel, CountryUpdateViewModel>
    , ICountryService
    {
        public CountryManager(IRepositoryAsync<Country> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
