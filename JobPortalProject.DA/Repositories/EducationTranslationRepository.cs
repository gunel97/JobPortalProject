using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class EducationTranslationRepository : EfCoreRepositoryAsync<EducationTranslation>, IEducationTranslationRepository
    {
        public EducationTranslationRepository(AppDbContext context) : base(context) { }
    }
}
