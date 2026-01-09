using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class CompanyDetailsAdminViewModel
    {
        public string UserId { get; set; } = null!;
        public string? Username { get; set; }
        public int TotalJobCount { get; set; }
        public int ExpiredJobCount { get; set; }
        public int TotalJobApplications { get; set; }
        public int AcceptedJobApplications { get; set; }
        public LanguageViewModel? SelectedLanguage { get; set; }
        public CompanyDetailsViewModel? Details { get; set; }
    }
}
