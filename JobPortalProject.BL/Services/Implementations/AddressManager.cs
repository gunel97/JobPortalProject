using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Security.Claims;

namespace JobPortalProject.BL.Services.Implementations
{
    public class AddressManager : CrudManager<Address, AddressViewModel, AddressCreateViewModel, AddressUpdateViewModel>
, IAddressService
    {
        private readonly ICookieService _cookieService;
        private readonly ICityService _cityService;
        private readonly IAddressTranslationService _addressTranslationService;

        public AddressManager(IRepositoryAsync<Address> repository, IMapper mapper, ICookieService cookieService, ICityService cityService, IAddressTranslationService addressTranslationService) : base(repository, mapper)
        {
            _cookieService = cookieService;
            _cityService = cityService;
            _addressTranslationService = addressTranslationService;
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

            var addressViewModels = addresses.Select(x => Mapper.Map<AddressViewModel>(x));

            return addressViewModels;
        }

        public async Task<List<AddressUpdateViewModel>> GetAddressUpdateViewModels(int companyId, int selectedLanguageId)
        {
            var addresses = await Repository.GetAllAsync(
                predicate: x => x.CompanyId == companyId,
                include: x => x.
                Include(a => a.AddressTranslations.Where(t => t.LanguageId == selectedLanguageId)).
                Include(c => c.City!).ThenInclude(ct => ct.CityTranslations.Where(t => t.LanguageId == selectedLanguageId)));
            var cityListItems = await _cityService.GetCitySelectListItemsWithCountry(selectedLanguageId);

            var addressUpdateViewModels = addresses.Select(x => new AddressUpdateViewModel
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                CityId = x.CityId,
                IsMainAddress = x.IsMainAddress,
                AddressTranslationId = x.AddressTranslations!.FirstOrDefault(x => x.LanguageId == selectedLanguageId)!.Id,
                Street = x.AddressTranslations.FirstOrDefault(x => x.LanguageId == selectedLanguageId)!.Street,
                CityListItems = cityListItems,

            }).ToList();

            return addressUpdateViewModels;
        }

        

        public async Task<bool> UpdateAddressAsync(int languageId, int addressId, AddressUpdateViewModel model)
        {
            var address = await Repository.GetAsync(predicate: x => x.Id == addressId,
                include: x => x.Include(a => a.AddressTranslations.Where(t => t.LanguageId == languageId)));

            if (address == null)
                return false;

            var translation = address.AddressTranslations.FirstOrDefault(x => x.LanguageId == languageId);
            if (translation == null)
                return false;

            var translationUpdateViewModel = new AddressTranslationUpdateViewModel
            {
                Id = translation.Id,
                Street = model.Street,
                AddressId = addressId,
                LanguageId = languageId,              
            };

            await _addressTranslationService.UpdateAsync(translation.Id, translationUpdateViewModel);

            address.CityId = model.CityId;
            await Repository.UpdateAsync(address);

            return true;
        }
    }
}
