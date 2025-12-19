using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class AddressRepository : EfCoreRepositoryAsync<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }
    }
}
