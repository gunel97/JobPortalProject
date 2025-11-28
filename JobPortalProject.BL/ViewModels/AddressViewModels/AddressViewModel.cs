using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
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

    public class AddressCreateViewModel { }

    public class AddressUpdateViewModel { }

    public class AddressTranslationViewModel { }

    public class AddressTranslationCreateViewModel { }
    public class AddressTranslationUpdateViewModel { }

}
