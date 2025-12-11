using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;

namespace JobPortalProject.DA.Repositories
{
    public class AddressRepository : EfCoreRepositoryAsync<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }
    }
    public class JobResponsibilityRepository : EfCoreRepositoryAsync<JobResponsibility>, IJobResponsibilityRepository
    {
        public JobResponsibilityRepository(AppDbContext context) : base(context) { }
    }
    public class JobResponsibilityTranslationRepository : EfCoreRepositoryAsync<JobResponsibilityTranslation>, IJobResponsibilityTranslationRepository
    {
        public JobResponsibilityTranslationRepository(AppDbContext context) : base(context) { }
    }
    public class JobMainDutyRepository : EfCoreRepositoryAsync<JobMainDuty>, IJobMainDutyRepository
    {
        public JobMainDutyRepository(AppDbContext context) : base(context) { }
    }
    public class JobMainDutyTranslationRepository : EfCoreRepositoryAsync<JobMainDutyTranslation>, IJobMainDutyTranslationRepository
    {
        public JobMainDutyTranslationRepository(AppDbContext context) : base(context) { }
    }
    public class JobExtraBenefitRepository : EfCoreRepositoryAsync<JobExtraBenefit>, IJobExtraBenefitRepository
    {
        public JobExtraBenefitRepository(AppDbContext context) : base(context) { }
    }
    public class JobExtraBenefitTranslationRepository : EfCoreRepositoryAsync<JobExtraBenefitTranslation>, IJobExtraBenefitTranslationRepository
    {
        public JobExtraBenefitTranslationRepository(AppDbContext context) : base(context) { }
    }
}
