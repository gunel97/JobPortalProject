using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class CompanyDashboardViewModel
    {
        public int CompanyId { get; set; }
        public string? Name { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsMembershipActive { get; set; }
        public DateTime? MembershipExpiresAt { get; set; }
        public int ActiveJobCount { get; set; }
        public int TotalApplicantCount { get; set; }
        public int WaitingInterviewCount { get; set; }
        public int TotalAcceptedCount { get; set; }
        public List<LanguageViewModel> Languages { get; set; } = [];
        public List<ApplicantOfCompanyViewModel> Applicants { get; set; } = [];
        public List<LanguageViewModel> ReadyLanguages { get; set; } = [];
        public List<LanguageViewModel> EmptyLanguages { get; set; } = [];
    }
}
