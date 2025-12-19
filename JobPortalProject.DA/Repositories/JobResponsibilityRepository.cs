using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobResponsibilityRepository : EfCoreRepositoryAsync<JobResponsibility>, IJobResponsibilityRepository
    {
        public JobResponsibilityRepository(AppDbContext context) : base(context) { }
    }
}
