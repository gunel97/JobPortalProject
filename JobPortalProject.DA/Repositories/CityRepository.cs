using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CityRepository : EfCoreRepositoryAsync<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context) { }
    }
}
