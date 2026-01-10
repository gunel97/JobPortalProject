using JobPortalProject.BL.Attributes;
using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.EducationViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobPortalProject.BL.ViewModels.ExperienceViewModels
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }
        public int ResumeId { get; set; } 
        public string CompanyName { get; set; } = null!;
        public string Responsibility { get; set; } = null!;
        public string Position { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ExperienceCreateViewModel
    {
        [Required(ErrorMessage ="Company name is required.")]
        public string? ExistedCompanyName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int ResumeId { get; set; }
        public ExperienceTranslationCreateViewModel Translation { get; set; } = null!;
    }

    public class ExperienceUpdateViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public List<ExperienceTranslationUpdateViewModel> Translations { get; set; } = [];
    }

    public class ExperienceUpdatePageViewModel
    {
        public CandidateDashboardViewModel? Dashboard { get; set; }
        public List<ExperienceUpdateViewModel> Models { get; set; } = [];
    }

    public class ExperienceAddViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
        public DateTime? EndDate { get; set; }
        public List<ExperienceTranslationCreateViewModel> Translations { get; set; } = [];
    }

    public class ExperienceTranslationViewModel
    {
        public int Id { get; set; }
        public int ExperienceId { get; set; }
        public string? CompanyName { get; set; }
        public string? Position { get; set; }
        public string? Responsibilty { get; set; }
    }

    public class ExperienceTranslationCreateViewModel
    {
        public int LanguageId { get; set; }
        public int ExperienceId { get; set; }
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; } = null!;
        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; } = null!;
        [Required(ErrorMessage = "Responsibility is required")]
        public string Responsibility { get; set; } = null!;
    }

    public class ExperienceTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public string? LangIcon { get; set; }
        public int LanguageId { get; set; }
        public int ExperienceId { get; set; }
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; } = null!;
        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; } = null!;

        [Required(ErrorMessage = "Responsibility is required")]
        public string Responsibility { get; set; } = null!;
    }

    public class ExperiencePageCreateViewModel
    {
        public int LanguageId { get; set; }
        public CandidateDashboardViewModel? DashboardViewModel { get; set; }
        public List<ExperienceCreateViewModel> Models { get; set; } = [];
    }
}
