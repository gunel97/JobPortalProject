using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobResponsibilityTranslationRepository : EfCoreRepositoryAsync<JobResponsibilityTranslation>, IJobResponsibilityTranslationRepository
    {
        public JobResponsibilityTranslationRepository(AppDbContext context) : base(context) { }
    }
}
