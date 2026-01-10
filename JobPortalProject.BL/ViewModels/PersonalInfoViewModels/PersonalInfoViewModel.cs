using JobPortalProject.BL.Attributes;
using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.PersonalInfoViewModels
{
    public class PersonalInfoViewModel
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        [Required(ErrorMessage ="Required firstname")]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ImageUrl { get; set; }=null!;
        public string PhoneNumber { get; set; } = null!;
        public string WorkEmail { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public AddressViewModel? Address { get; set; }
    }

    public class PersonalInfoCreateViewModel
    {
        public int LanguageId { get; set; }
        public int ResumeId { get; set; }
        [Required(ErrorMessage = "WorkEmail is required")]
        public string? WorkEmail { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
        public int GenderId { get; set; }
        //public Gender Gender { get; set; }
        public List<SelectListItem> GenderItems { get; set; } = [];
        [Required(ErrorMessage = "Profile Image is required")]
        public IFormFile? ImageFile {  get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        [Required(ErrorMessage = "Birth Date is required")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Minimum age is 16.")]
        public DateTime BirthDate { get; set; }
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class PersonalInfoUpdateViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> CitiesList { get; set; } = [];
        public int LanguageId { get; set; }
        public int ResumeId { get; set;}
        public string? WorkEmail { get; set; }
        public string? PhoneNumber { get; set;}
        public int GenderId { get; set; }
        public string? Gender { get; set; }
        public List<SelectListItem> GenderItems { get; set; } = [];
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class PersonalInfoTranslationViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
    }

    public class PersonalInfoTranslationCreateViewModel 
    {
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string Firstname { get; set; } = null!;
        [Required(ErrorMessage = "Lastname is required")]
        public string Lastname { get; set; } = null!;
        public int PersonalInfoId { get; set; }
    }

    public class PersonalInfoTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int PersonalInfoId { get; set; }
    }

}
