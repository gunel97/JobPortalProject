using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.PersonalInfoViewModels
{
    public class PersonalInfoViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WorkEmail { get; set; }
        public int GenderId { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public AddressViewModel? Address { get; set; }
    }

    public class PersonalInfoCreateViewModel
    {
        public int LanguageId { get; set; }
        public int ResumeId { get; set; }
        public string? WorkEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public int GenderId { get; set; }
        //public Gender Gender { get; set; }
        public List<SelectListItem> GenderItems { get; set; } = [];
        public IFormFile? ImageFile {  get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        public DateTime BirthDate { get; set; }
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class PersonalInfoUpdateViewModel
    {
        public int Id { get; set; }
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
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public int PersonalInfoId { get; set; }
    }

    public class PersonalInfoTranslationUpdateViewModel { }

    public class ProfileCreateViewModel
    {
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> CitiesList { get; set; } = [];
        public string? Street { get; set; }
        public PersonalInfoTranslationCreateViewModel personalInfoTranslationModel { get; set; } = null!;
        public ResumeTranslationCreateViewModel resumeTranslationModel { get; set; }= null!;
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class ProfileUpdateViewModel
    {
        public int LanguageId { get; set; }
         }

    public class ProfileTranslationCreateViewModel
    {
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public PersonalInfoTranslationCreateViewModel personalInfoTranslationModel { get; set; } = null!;
        public ResumeTranslationCreateViewModel resumeTranslationModel { get; set; } = null!;
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class ProfileTranslationUpdateViewModel
    {

    }

}
