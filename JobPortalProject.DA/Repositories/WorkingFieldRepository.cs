using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class WorkingFieldRepository : EfCoreRepositoryAsync<WorkingField>, IWorkingFieldRepository
    {
        public WorkingFieldRepository(AppDbContext context) : base(context) { }
    }
}
