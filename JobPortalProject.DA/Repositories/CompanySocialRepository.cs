using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CompanySocialRepository : EfCoreRepositoryAsync<CompanySocial>, ICompanySocialRepository
    {
        public CompanySocialRepository(AppDbContext context) : base(context) { }
    }
}
