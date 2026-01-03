using JobPortalProject.BL.ViewModels.CandidateViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
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

    public class JobApplicationsOfCandidateFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "PostedDate";
        public string SortOrder { get; set; } = "desc";
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 5;
    }

    public class AppliedJobsOfCandidatePageViewModel
    {
        public JobApplicationsOfCandidateFilterViewModel? Filter { get; set; }
        public PagedResultModel<JobApplicationsOfCandidateViewModel> JobApplicationsModels { get; set; } = null!;
        public CandidateDashboardViewModel? Dashboard { get; set; }
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

    public class ApplicantOfCompanyViewModel
    {
        public int ApplicationId { get; set; }
        public int CandidateId { get; set; }
        public int JobId { get; set; }
        public string? CandidateName { get; set; }
        public string? JobTitle { get; set; }
        public DateTime BirthDateOfCandidate { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime PostedAt { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
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

   
}
