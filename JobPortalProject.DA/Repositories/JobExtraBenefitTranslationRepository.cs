using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class JobExtraBenefitTranslationRepository : EfCoreRepositoryAsync<JobExtraBenefitTranslation>, IJobExtraBenefitTranslationRepository
    {
        public JobExtraBenefitTranslationRepository(AppDbContext context) : base(context) { }
    }
}
