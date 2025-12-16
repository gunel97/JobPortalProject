using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.AddressViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool IsMainAddress { get; set; }
        public string? Street { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
        public CityViewModel? City {get;set;}
    }

    public class AddressCreateViewModel
    {
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        public int SelectedLanguageId { get;set; }
        public int CompanyTranslationsCount { get; set; }
        public bool IsMainAddress { get; set; }
        public List<SelectListItem> CityListItems { get; set; } = [];
        public List<AddressTranslationCreateViewModel> AddressTranslationCreateViewModels { get; set; } = [];
    }

    public class AddressUpdateViewModel
    {
        public int Id { get; set; }
        public bool IsMainAddress { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> CityListItems { get; set; } = [];
        public int CompanyId { get; set; }
        public int AddressTranslationId { get; set; }
        public string? Street { get; set; }
    }

    public class AddressTranslationViewModel { }

    public class AddressTranslationCreateViewModel
    {
        public int AddressId { get; set; }
        public int LanguageId { get; set; }
        public string? ExistingAddress { get; set; }
        public string Street { get; set; } = null!;
    }

    public class AddressTranslationUpdateViewModel {
        public int Id { get; set; }
        public int LanguageId {  get; set; }
        public int AddressId { get; set; }
        public string? Street { get; set; }
    }

}
