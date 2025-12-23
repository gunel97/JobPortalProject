using JobPortalProject.BL.ViewModels.CandidateViewModels;
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
    }

    public class EducationCreateViewModel
    {
        public int IdForTranslation { get; set; }
        public int EducationTypeId { get; set; }
        public string? MajorName { get; set; }
        public string? SchoolName { get; set; }
        public int LanguageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EducationAddViewModel
    {
        public int ResumeId { get; set; }
        public int EducationTypeId { get; set; }
        public List<SelectListItem> EducationTypes { get; set; } = [];
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public List<EducationTranslationCreateViewModel> Translations { get; set; } = [];
    }

    public class EducationUpdateViewModel
    {
        public int Id { get; set; }
        public int EducationTypeId { get; set; }
        public List<SelectListItem> EducationTypes { get; set; } = [];
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public List<EducationTranslationUpdateViewModel> Translations { get; set; } = [];
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }

    public class EducationTranslationViewModel { }

    public class EducationTranslationCreateViewModel
    {
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        public string? SchoolName { get; set; }
        public string? MajorName { get; set; }
    }

    public class EducationTranslationUpdateViewModel
    {
        public string? LangIcon { get; set; }
        public int Id { get; set; }
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        public string SchoolName { get; set; } = null!;
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
