using JobPortalProject.BL.ViewModels.CandidateViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.ExperienceViewModels
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ResumeId { get; set; }
        public List<ExperienceTranslationViewModel> Translations { get; set; } = [];
    }

    public class ExperienceCreateViewModel
    {
        public string? ExistedCompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ResumeId { get; set; }
        public ExperienceTranslationCreateViewModel Translation { get; set; } = null!;
    }

    public class ExperienceUpdateViewModel { }

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
        public string CompanyName { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Responsibility { get; set; } = null!;
    }

    public class ExperienceTranslationUpdateViewModel { }

    public class ExperiencePageCreateViewModel
    {
        public int LanguageId { get; set; }
        public CandidateDashboardViewModel? DashboardViewModel { get; set; }
        public List<ExperienceCreateViewModel> Models { get; set; } = [];
    }
}
