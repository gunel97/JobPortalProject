using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobRepository : EfCoreRepositoryAsync<Job>, IJobRepository
    {
        public JobRepository(AppDbContext context) : base(context) { }
    }
}
