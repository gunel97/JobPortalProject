using JobPortalProject.BL.ViewModels.CountryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CityViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public CountryViewModel? Country { get; set; }
        public string? CountryName {  get; set; } 
    }

    public class CityCreateViewModel { }

    public class CityUpdateViewModel { }

    public class CityTranslationViewModel
    {
    }

    public class CityTranslationCreateViewModel { }

    public class CityTranslationUpdateViewModel { }
}
