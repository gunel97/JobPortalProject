using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobMainDutyTranslationRepository : EfCoreRepositoryAsync<JobMainDutyTranslation>, IJobMainDutyTranslationRepository
    {
        public JobMainDutyTranslationRepository(AppDbContext context) : base(context) { }
    }
}
