using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CandidateViewModels
{
    public class CandidateViewModel
    {
        public int Id { get; set; }
        
    }

    public class CandidateCreateViewModel {
        public string AppUserId { get; set; } = null!;
    }

    public class CandidateUpdateViewModel { }

    public class CandidateDashboardViewModel
    {
        public int ResumeId { get; set; }
        public string? UserName { get; set; }
        public List<LanguageViewModel> EmptyLanguages { get; set; } = new List<LanguageViewModel>();
        public List<LanguageViewModel> ReadyLanguages { get; set; } = new List<LanguageViewModel>();
    }
 
}
