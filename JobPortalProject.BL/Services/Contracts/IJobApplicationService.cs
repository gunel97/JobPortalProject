using JobPortalProject.BL.ViewModels.JobApplicationViewModels;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IJobApplicationService:ICrudService<JobApplication, JobApplicationsOfCandidateViewModel,JobApplicationCreateViewModel, JobApplicationUpdateViewModel>
    {
        public Task<bool> CancelJobApplication(int jobId, int candidateId);
        public Task<bool> CheckIfJobApplied(int jobId);
        public Task<bool> ApplyJob(int jobId, int candidateId);
        public Task<List<JobApplication>> GetAppliedJobsOfCandidate(int candidateId);
        public Task<AppliedJobsOfCandidatePageViewModel> GetAppliedJobsPageOfCandidateViewModel(int candidateId);
        public Task<List<JobApplication>> GetApplicantsOfJob(int jobId);
        public Task<ApplicantsOfJobViewModel> GetApplicantsViewModel(int jobId);
    }
}
