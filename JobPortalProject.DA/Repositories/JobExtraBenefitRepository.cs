using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobExtraBenefitRepository : EfCoreRepositoryAsync<JobExtraBenefit>, IJobExtraBenefitRepository
    {
        public JobExtraBenefitRepository(AppDbContext context) : base(context) { }
    }
}
