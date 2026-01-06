using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CompanyViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public int CompanySize { get; set; }
        public int ActiveJobCount { get; set; }
        public int TranslationsCount { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public string? LogoUrl {  get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string? CategoryName { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsMembershipActive { get; set; }
        public DateTime MemberSince { get; set; }
        public DateTime? LastPostedJob {  get; set; }
        public DateTime? MembershipExpiresAt { get; set; }
        public AddressViewModel MainAddress { get; set; } = null!;
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<WorkingFieldViewModel> WorkingFields { get; set; } = [];
        public List<LanguageViewModel> ReadyLanguages { get; set; } = [];
        public List<LanguageViewModel> EmptyLanguages { get; set; } = [];
    }

    public class CompanyFilterViewModel
    {
        public List<int> TypeIds { get; set; } = [];
        public List<int> CityIds { get; set; } = [];
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "title";
        public string SortOrder { get; set; } = "desc";
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 10;
    }

    public class CompanyCreateViewModel
    {
        public string? AppUserId { get; set; }
        public bool IsAccountApproved { get; set; } = false;
        public int CompanyTypeId { get; set; }
        public DateTime MemberSince { get; set; } = DateTime.UtcNow;
    }

    public class CompanyUpdateViewModel
    {
        public int Id { get; set; }
        public int SelectedUpdateLanguageId { get; set; }
        public int CompanySize { get; set; }
        public string? CompanyEmail { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public bool IsAccountApproved { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public IFormFile? CoverPhotoFile { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public int CompanyTypeId { get; set; }
        public int MainAddressId { get; set; }
        public List<LanguageViewModel> EmptyLanguages { get; set; } = []; 
        public List<SelectListItem> AddressesOfCompany { get; set; } = [];
        public List<CompanyTranslationUpdateViewModel> CompanyTranslations { get; set; } = [];
        public List<SelectListItem> CompanyTypeList { get; set; } = [];
        public List<SelectListItem> SocialMediasList { get; set; } = [];
        public List<SelectListItem> CitiesList { get; set; } = [];
        public List<CompanySocialUpdateViewModel> CompanySocialUpdateViewModels { get; set; } = [];       
    }

    public class CompanyTranslationViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public int LanguageId { get; set; }       
    }

    public class CompanyTranslationCreateViewModel
    {
        public  int LanguageId { get; set; }
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CompanyTranslationUpdateViewModel
    {
        public int TranslationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
        public int CompanyId { get; set; }
        public string? LanguageIcon { get; set; }
    }

    public class CompanyTranslationEditPageViewModel
    {
        public int LangaugeId { get; set; }
        public CompanyTranslationUpdateViewModel CompanyTranslationUpdateViewModel { get; set; } = null!;
        public List<WorkingFieldUpdateViewModel> WorkingFieldUpdateViewModels { get; set; } = [];
        public List<AddressUpdateViewModel> AddressUpdateViewModels { get; set; } = [];
    }

    public class AddTranslationToExistedCompanyViewModel
    {
        public int CompanyId { get; set; }
        public int LanguageId { get; set; }
        public CompanyTranslationCreateViewModel translationModel { get; set; } = null!;
        public List<WorkingFieldTranslationCreateViewModel> workingFieldTranslationCreateModels { get; set; } = [];
        public List<AddressTranslationCreateViewModel> addressTranslationCreateModels { get; set; } = [];
    }

}
