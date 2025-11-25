using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.BL.Services.Implementations
{
    public class CityManager : CrudManager<City, CityViewModel, CityCreateViewModel, CityUpdateViewModel>
, ICityService
    {
        public CityManager(IRepositoryAsync<City> repository, IMapper mapper) : base(repository, mapper) { }
    }


}
