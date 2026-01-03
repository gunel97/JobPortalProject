using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobApplicationService:ICrudService<JobApplication, JobApplicationsOfCandidateViewModel,JobApplicationCreateViewModel, JobApplicationUpdateViewModel>
    {
        public Task<List<ApplicantOfCompanyViewModel>> GetApplicantsOfCompany(int companyId);
        public Task<List<JobApplicationsOfCandidateViewModel>> GetAppliedJobModelsOfCandidate(int candidateId);
        public Task<PagedResultModel<JobApplicationsOfCandidateViewModel>> GetPagedAppliedJobsOfCandidate(JobApplicationsOfCandidateFilterViewModel filter, int candidateId);
        public Task<bool> InterviewJobApplication(int jobId, int candidateId);
        public Task<bool> AcceptJobApplication(int jobId, int candidateId);
        public Task<bool> RejectJobApplication(int jobId, int candidateId);
        public Task<bool> CancelJobApplication(int jobId, int candidateId);
        public Task<bool> CheckIfJobApplied(int jobId);
        public Task<bool> ApplyJob(int jobId, int candidateId);
        public Task<List<JobApplication>> GetAppliedJobsOfCandidate(int candidateId);
        public Task<AppliedJobsOfCandidatePageViewModel> GetAppliedJobsPageOfCandidateViewModel(JobApplicationsOfCandidateFilterViewModel filter, int candidateId);
        public Task<List<JobApplication>> GetApplicantsOfJob(int jobId);
        public Task<ApplicantsOfJobViewModel> GetApplicantsViewModel(int jobId);
    }
}
