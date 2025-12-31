using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CompanyTypeViewModels
{
    public class CompanyTypeViewModel
    {
        public int Id { get;set; }
        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public string? Name { get;set; }
        public DateTime CreatedAt { get; set; }
        public List<int> CompanyIds { get; set; } = [];
    }

    public class CompanyTypeFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Size { get; set; } = 10;
        public int Index { get; set; } = 0;
    }

    public class CompanyTypePagedViewModel
    {
        public PagedResultModel<CompanyTypeViewModel> CompanyTypes { get; set; } = null!;
    }

    public class CompanyTypeCreateViewModel 
    {
        public List<CompanyTypeTranslationCreateViewModel> CompanyTypeTranslations { get; set; } = [];
    }

    public class CompanyTypeUpdateViewModel
    {
        public int Id { get; set; }
        public List<CompanyTypeTranslationUpdateViewModel> Translations { get; set; } = [];
    }

    public class CompanyTypeDetailsViewModel
    {
        public int Id { get; set; }
        public List<CompanyTypeTranslationViewModel> Translations { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CompanyCount { get; set; }
    }

    public class CompanyTypeTranslationViewModel
    {
        public int Id { get; set; }
        public int CompanyTypeId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? LanguageIcon { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CompanyTypeTranslationCreateViewModel
    {
        public int CompanyTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = null!;
        public string? LanguageIcon { get; set; }
    }

    public class CompanyTypeTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int CompanyTypeId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? LanguageIcon { get; set; }
    }
}
