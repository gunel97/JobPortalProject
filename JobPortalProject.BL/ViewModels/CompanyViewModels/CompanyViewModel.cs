using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
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
        public DateTime? MemberSince { get; set; }
        public DateTime? LastPostedJob {  get; set; }
        public AddressViewModel? MainAddress { get; set; }
        public List<AddressViewModel> Addresses { get; set; } = [];
        public List<WorkingFieldViewModel> WorkingFields { get; set; } = [];
    }

    public class CompanyCreateViewModel
    {
        public required int CompanyTypeId { get; set; }
        public required string AppUserId { get; set; }
    }

    public class CompanyUpdateViewModel { }

    public class CompanyTranslationViewModel
    {
      
    }

    public class CompanyTranslationCreateViewModel
    {
        public required int LanguageId { get; set; }
        public required string Name { get; set; }
        public required int CompanyId { get; set; }
    }

    public class CompanyTranslationUpdateViewModel { }


}
