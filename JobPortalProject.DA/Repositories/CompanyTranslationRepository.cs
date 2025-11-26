using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CompanyTranslationRepository : EfCoreRepositoryAsync<CompanyTranslation>, ICompanyTranslationRepository
    {
        public CompanyTranslationRepository(AppDbContext context) : base(context) { }
    }

}
