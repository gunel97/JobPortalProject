using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CountryTranslationRepository : EfCoreRepositoryAsync<CountryTranslation>, ICountryTranslationRepository
    {
        public CountryTranslationRepository(AppDbContext context) : base(context) { }
    }
}
