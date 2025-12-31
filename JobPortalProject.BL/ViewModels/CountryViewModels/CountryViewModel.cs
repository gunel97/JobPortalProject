using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CountryViewModels
{
    public class CountryFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
    }

    public class CountryPagedViewModel
    {
        public PagedResultModel<CountryViewModel> Countries { get; set; } = null!;
    }

    public class CountryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CityCount { get; set; }
        public int CompanyAddressCount { get; set; }
        public int CompanyCount { get; set; }
        public int CandidateCount { get; set; }
    }

    public class CountryDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CountryTranslationViewModel> Translations { get; set; } = [];
    }

    public class CountryCreateViewModel
    {
        public List<CountryTranslationCreateViewModel> Translations { get; set; } = [];
    }

    public class CountryUpdateViewModel
    {
        public int Id { get; set; }
        public List<CountryTranslationUpdateViewModel> Translations { get; set; } = [];
    }

    public class CountryTranslationViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public string? Name { get; set; }
        public string? LanguageIcon { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CountryTranslationCreateViewModel
    {
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CountryTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = null!;
        public string? LanguageIcon { get; set; }
    }
}
