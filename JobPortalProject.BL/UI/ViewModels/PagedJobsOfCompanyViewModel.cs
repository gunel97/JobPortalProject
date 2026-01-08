using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class PagedJobsOfCompanyViewModel
    {
        public JobFilterViewModel? Filter { get; set; }
        public PagedResultModel<JobViewModel> Jobs { get; set; } = null!;
        public List<LanguageViewModel> ReadyLanguages { get; set; } = [];
        public List<LanguageViewModel> EmptyLanguages { get; set; } = [];
        public bool IsAccountActive { get; set; }
    }
}
