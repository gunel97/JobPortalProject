using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class PersonalInfoRepository : EfCoreRepositoryAsync<PersonalInfo>, IPersonalInfoRepository
    {
        public PersonalInfoRepository(AppDbContext context) : base(context) { }
    }
}
