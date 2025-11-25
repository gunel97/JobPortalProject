using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CountryRepository : EfCoreRepositoryAsync<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context) : base(context) { }
    }
}
