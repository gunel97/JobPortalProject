using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CompanyTypeTranslationRepository : EfCoreRepositoryAsync<CompanyTypeTranslation>, ICompanyTypeTranslationRepository
    {
        public CompanyTypeTranslationRepository(AppDbContext context) : base(context) { }
    }

}
