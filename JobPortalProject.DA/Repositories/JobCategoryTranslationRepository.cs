using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobCategoryTranslationRepository : EfCoreRepositoryAsync<JobCategoryTranslation>, IJobCategoryTranslationRepository
    {
        public JobCategoryTranslationRepository(AppDbContext context) : base(context) { }
    }
}
