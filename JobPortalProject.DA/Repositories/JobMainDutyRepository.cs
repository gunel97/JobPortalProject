using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobMainDutyRepository : EfCoreRepositoryAsync<JobMainDuty>, IJobMainDutyRepository
    {
        public JobMainDutyRepository(AppDbContext context) : base(context) { }
    }
}
