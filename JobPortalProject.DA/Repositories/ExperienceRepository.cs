using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class ExperienceRepository : EfCoreRepositoryAsync<Experience>, IExperienceRepository
    {
        public ExperienceRepository(AppDbContext context) : base(context) { }
    }
}
