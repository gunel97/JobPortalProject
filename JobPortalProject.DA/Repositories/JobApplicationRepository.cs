using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobApplicationRepository : EfCoreRepositoryAsync<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(AppDbContext context) : base(context) { }
    }
}
