using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobCategoryRepository : EfCoreRepositoryAsync<JobCategory>, IJobCategoryRepository
    {
        public JobCategoryRepository(AppDbContext context) : base(context) { }
    }
}
