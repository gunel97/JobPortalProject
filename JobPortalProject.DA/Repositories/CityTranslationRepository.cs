using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CityTranslationRepository : EfCoreRepositoryAsync<CityTranslation>, ICityTranslationRepository
    {
        public CityTranslationRepository(AppDbContext context) : base(context) { }
    }
}
