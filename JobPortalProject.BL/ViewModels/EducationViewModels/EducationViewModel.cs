using JobPortalProject.BL.Attributes;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.EducationViewModels
{
    public class EducationViewModel
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string MajorName { get; set; } = null!;
        public string EducationType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class EducationCreateViewModel
    {
        public int IdForTranslation { get; set; }
        public int EducationTypeId { get; set; }
        public EducationType EducationType { get; set; }
        [Required(ErrorMessage ="Major name is Required")]
        public string? MajorName { get; set; }
        [Required(ErrorMessage = "School name is Required")]
        public string? SchoolName { get; set; }
        public int LanguageId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }

    public class EducationAddViewModel
    {
        public int ResumeId { get; set; }
        public int EducationTypeId { get; set; }
        public List<SelectListItem> EducationTypes { get; set; } = [];
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public List<EducationTranslationCreateViewModel> Translations { get; set; } = [];
    }

    public class EducationUpdateViewModel
    {
        public int Id { get; set; }
        public int EducationTypeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        public DateTime? EndDate { get; set; }
        public List<EducationTranslationUpdateViewModel> Translations { get; set; } = [];
    }

    public class EducationUpdatePageViewModel
    {
        public List<SelectListItem> EducationTypes { get; set; } = [];
        public CandidateDashboardViewModel? DashboardModel { get; set; }
        public List<EducationUpdateViewModel> UpdateModels { get; set; } = [];
    }

    public class EducationTranslationViewModel { }

    public class EducationTranslationCreateViewModel
    {
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        [Required (ErrorMessage ="School name is required")]
        public string? SchoolName { get; set; }
        [Required(ErrorMessage = "Major name is required")]
        public string? MajorName { get; set; }
    }

    public class EducationTranslationUpdateViewModel
    {
        public string? LangIcon { get; set; }
        public int Id { get; set; }
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "School name is required")]
        public string SchoolName { get; set; } = null!;
        [Required(ErrorMessage = "Major name is required")]
        public string MajorName { get; set; } = null!;
    }

    public class EducationPageCreateViewModel
    {
        public int LanguageId { get; set; }
        public List<SelectListItem> EducationTypes { get; set; } = [];
        public List<EducationCreateViewModel> Models { get; set; } = [];
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }
}
