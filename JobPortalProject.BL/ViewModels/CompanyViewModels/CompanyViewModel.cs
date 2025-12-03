using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string? Name { get; set; }
        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";
        public string? LogoUrl {  get; set; }
        public string? CoverPhotoUrl { get; set; }
        public string? CategoryName { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public bool IsAccountActive { get; set; }
        public DateTime? MemberSince { get; set; }
        public DateTime? LastPostedJob {  get; set; }
        public AddressViewModel? MainAddress { get; set; }
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<WorkingFieldViewModel> WorkingFields { get; set; } = [];
    }

    public class CompanyCreateViewModel
    {
        public string? AppUserId { get; set; }
        public bool IsAccountApproved { get; set; } = false;
        public int? CompanyTypeId { get; set; }
        public List<SelectListItem>? CompanyTypeList { get; set; } = []; 
    }

    public class CompanyUpdateViewModel
    {
        public int Id { get; set; }
        public int CompanySize { get; set; }
        public string? CompanyEmail { get; set; }
        public bool IsAccountApproved { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public IFormFile? CoverPhotoFile { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public int CompanyTypeId { get; set; }
        public List<SelectListItem>? CompanyTypeList { get; set; } = [];
        public List<CompanySocialUpdateViewModel> CompanySocialUpdateViewModels { get; set; } = [];
        public CompanyTranslationUpdateViewModel? CompanyTranslationUpdateViewModel { get; set; } 
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
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
        public int CompanyId { get; set; }
    }


}
