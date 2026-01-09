using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class MainSocialRepository : EfCoreRepositoryAsync<MainSocial>, IMainSocialRepository
    {
        public MainSocialRepository(AppDbContext context) : base(context) { }
    }
}
