using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class AddressTranslationRepository : EfCoreRepositoryAsync<AddressTranslation>, IAddressTranslationRepository
    {
        public AddressTranslationRepository(AppDbContext context) : base(context) { }
    }
}
