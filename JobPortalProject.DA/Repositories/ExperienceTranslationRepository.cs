using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class ExperienceTranslationRepository : EfCoreRepositoryAsync<ExperienceTranslation>, IExperienceTranslationRepository
    {
        public ExperienceTranslationRepository(AppDbContext context) : base(context) { }
    }
}
