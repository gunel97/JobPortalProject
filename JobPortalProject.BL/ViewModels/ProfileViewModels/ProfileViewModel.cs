using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.ProfileViewModels
{
    public class ProfileCreateViewModel
    {
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public List<SelectListItem> CitiesList { get; set; } = [];
        public string? Street { get; set; }
        public PersonalInfoTranslationCreateViewModel personalInfoTranslationModel { get; set; } = null!;
        public ResumeTranslationCreateViewModel resumeTranslationModel { get; set; } = null!;
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class ProfileUpdateViewModel
    {
        public List<ProfileTranslationUpdateViewModel> ProfileTranslations { get; set; } = [];
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class ProfileTranslationCreateViewModel
    {
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public string? City { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Street { get; set; }
        public PersonalInfoTranslationCreateViewModel personalInfoTranslationModel { get; set; } = null!;
        public ResumeTranslationCreateViewModel resumeTranslationModel { get; set; } = null!;
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class ProfileTranslationUpdateViewModel
    {
        public string? Icon { get; set; }
        public int LanguageId { get; set; }
        public string Street { get; set; } = null!;
        public PersonalInfoTranslationUpdateViewModel PersonalInfoTranslation { get; set; } = null!;
        public ResumeTranslationUpdateViewModel ResumeTranslation { get; set; } = null!;
    }
}
