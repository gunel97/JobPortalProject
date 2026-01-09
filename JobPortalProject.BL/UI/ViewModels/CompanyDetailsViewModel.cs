using JobPortalProject.BL.ViewModels.CompanySocialViewModels;
using JobPortalProject.BL.ViewModels.CompanyViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
namespace JobPortalProject.BL.UI.ViewModels
{
    public class CompanyDetailsViewModel
    {
        public CompanyViewModel? Company { get; set; }
        public List<CompanySocialViewModel>? CompanySocials { get; set; } = [];
        public List<JobViewModel> ActiveJobs { get; set; } = [];
        public List<LanguageViewModel> ReadyLanguages { get; set; } = []; 
        public bool IsAccountApproved { get; set; }
    }
}
