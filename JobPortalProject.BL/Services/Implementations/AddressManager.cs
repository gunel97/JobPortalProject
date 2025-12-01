using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace JobPortalProject.BL.Services.Implementations
{
    public class AddressManager : CrudManager<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
, IAddressService
    {
        private readonly ICookieService _cookieService;
        public AddressManager(IRepositoryAsync<Address> repository, IMapper mapper, ICookieService cookieService) : base(repository, mapper)
        {
            _cookieService = cookieService;
        }

        public override async Task<IEnumerable<AddressViewModel>> GetAllAsync(Expression<Func<Address, bool>>? predicate = null, Func<IQueryable<Address>, IOrderedQueryable<Address>>? orderBy = null, Func<IQueryable<Address>, IIncludableQueryable<Address, object>>? include = null, bool AsNoTracking = false)
        {
            var language = await _cookieService.GetLanguageAsync();
            int languageId = language.Id;
            var addresses = await Repository.GetAllAsync(
                                            //predicate: x => !x.IsDeleted && x.CompanyAddresses.Any(),
                                            include: x => x
                                            .Include(at => at.AddressTranslations!.Where(at => at.LanguageId == languageId))
                                            .Include(a => a.City!).ThenInclude(c => c.CityTranslations!.Where(a => a.LanguageId == languageId))
                                            .Include(a => a.City!).ThenInclude(c => c.Country!).ThenInclude(ct => ct.Translations!
                                            .Where(a => a.LanguageId == languageId)));

            var addressViewModels = addresses.Select(x=>Mapper.Map<AddressViewModel>(x));

            return addressViewModels;

        }
    }
}
