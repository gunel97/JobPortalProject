using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class WorkingFieldTranslationRepository : EfCoreRepositoryAsync<WorkingFieldTranslation>, IWorkingFieldTranslationRepository
    {
        public WorkingFieldTranslationRepository(AppDbContext context) : base(context) { }
    }
}
