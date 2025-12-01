using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobTranslationRepository : EfCoreRepositoryAsync<JobTranslation>, IJobTranslationRepository
    {
        public JobTranslationRepository(AppDbContext context) : base(context) { }
    }
}
