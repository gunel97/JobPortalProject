using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class ResumeTranslationRepository : EfCoreRepositoryAsync<ResumeTranslation>, IResumeTranslationRepository
    {
        public ResumeTranslationRepository(AppDbContext context) : base(context) { }
    }
}
