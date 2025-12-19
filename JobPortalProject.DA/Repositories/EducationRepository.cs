using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class EducationRepository : EfCoreRepositoryAsync<Education>, IEducationRepository
    {
        public EducationRepository(AppDbContext context) : base(context) { }
    }
}
