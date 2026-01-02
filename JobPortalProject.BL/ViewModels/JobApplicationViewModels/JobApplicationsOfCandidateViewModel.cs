using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobApplicationViewModels
{
    public class JobApplicationsOfCandidateViewModel
    {
        public int JobApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobDetailsUrl => $"{JobTitle?.Replace(" ", "-").Replace("/", "-")}-{JobId}";
        public string CompanyDetailsUrl => $"{CompanyName?.Replace(" ", "-").Replace("/", "-")}-{CompanyId}";

        public int CompanyId { get; set; }
        public int CandidateId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobAddress { get; set; }
        public string? CompanyName {  get; set; }
        public string? CompanyLogo { get; set; }
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
        public string? SalaryType { get; set; }
        public DateTime JobCreatedAt { get; set; }
        public DateTime AppliedAt { get; set; }
        public string? Status { get; set; }
    }

    public class ApplicantOfJobViewModel
    {
        public int JobApplicationId { get; set; }
        public string? ResumeViewModel { get; set; }
        public int CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateImageUrl { get; set; }
        public DateTime CandidateBirthDate { get; set; }
        public DateTime ApplyDate { get; set; }
        public string? Status { get; set; }
        public ResumeViewModel ResumeModel { get; set; } = null!;
    }

    public class ApplicantsOfJobViewModel
    {
        public List<ApplicantOfJobViewModel> Applications { get; set; } = [];
        public JobViewModel Job { get; set; } = null!;
        public string? JobTitle { get; set; }
        public DateTime JobPostedDate { get; set; }
        public DateTime JobExpireDate { get; set; }
    }

    public class JobApplicationCreateViewModel
    {
        public int CandidateId { get; set; }
        public int JobId { get; set; }
        public JobApplicationStatus JobStatus { get; set; } = (JobApplicationStatus)1;
    }

    public class JobApplicationUpdateViewModel
    {

    }

    public class AppliedJobsOfCandidatePageViewModel
    {
        public List<JobApplicationsOfCandidateViewModel> JobApplicationsModels { get; set; } = [];
        public CandidateDashboardViewModel? Dashboard { get; set; }
    }
}
