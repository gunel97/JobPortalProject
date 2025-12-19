using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class CandidateRepository : EfCoreRepositoryAsync<Candidate>, ICandidateRepository
    {
        public CandidateRepository(AppDbContext context) : base(context) { }
    }
}
