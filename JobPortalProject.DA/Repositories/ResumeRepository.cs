using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class ResumeRepository : EfCoreRepositoryAsync<Resume>, IResumeRepository
    {
        public ResumeRepository(AppDbContext context) : base(context) { }
    }
}
