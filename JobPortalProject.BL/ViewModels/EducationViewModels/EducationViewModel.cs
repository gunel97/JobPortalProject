using JobPortalProject.BL.ViewModels.CandidateViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
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


    public class EducationUpdateViewModel { }

    public class EducationTranslationViewModel { }

    public class EducationTranslationCreateViewModel
    {
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        public string? SchoolName { get; set; }
        public string? MajorName { get; set; }
    }

    public class EducationTranslationUpdateViewModel { }

    public class EducationPageCreateViewModel
    {
        public int LanguageId { get; set; }
        public List<SelectListItem> EducationTypes { get; set; } = [];
        public List<EducationCreateViewModel> Models { get; set; } = [];
        public CandidateDashboardViewModel? DashboardModel { get; set; }
    }
}
