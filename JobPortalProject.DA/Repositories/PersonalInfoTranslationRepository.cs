using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class PersonalInfoTranslationRepository : EfCoreRepositoryAsync<PersonalInfoTranslation>, IPersonalInfoTranslationRepository
    {
        public PersonalInfoTranslationRepository(AppDbContext context) : base(context) { }
    }
}
