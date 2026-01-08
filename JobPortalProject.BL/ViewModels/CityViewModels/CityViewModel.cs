using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CityViewModels
{
    public class CityFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
        public int? CountryId { get; set; }
    }

    public class CityPagedViewModel
    {
       public PagedResultModel<CityViewModel> Cities { get; set; } = null!;
    }

    public class CityDetailsViewModel
    {
        public int Id { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CityTranslationViewModel> CityTranslations { get; set; } = [];
    }

    public class CityViewModel
    {
        public int Id { get; set; }
        public int ActiveJobCount { get; set; }
        public string? Name { get; set; }
        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public string? CoverPhotoUrl { get; set; }
        public CountryViewModel? Country { get; set; }
        public int AddressCount { get; set; }
        public int CompanyAddressCount { get; set; }
        public int CompanyCount { get; set; }
        public int CandidateCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CityCreateViewModel 
    {
        public IFormFile ImageFile { get; set; } = null!; 
        public string? CoverPhotoUrl { get; set; }
        public string? CoverPhotoPublicId { get; set; }
        public int CountryId { get; set; }
        public List<CityTranslationCreateViewModel> CityTranslations { get; set; } = [];

    }

    public class CityUpdateViewModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string? CoverPhotoPublicId { get; set; }
        public List<SelectListItem> CountryItems { get; set; } = [];
        public List<CityTranslationUpdateViewModel> CityTranslations { get; set; } = [];
    }


    public class CityTranslationViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? LanguageIcon { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CityTranslationCreateViewModel
    {
        public int LanguageId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CityTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? LanguageIcon { get; set; }
    }
}
