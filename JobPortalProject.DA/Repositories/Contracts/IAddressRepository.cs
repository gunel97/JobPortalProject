using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.DA.Repositories.Contracts
{
    public interface IAddressRepository : IRepositoryAsync<Address> { }
    public interface IJobResponsibilityRepository : IRepositoryAsync<JobResponsibility> { }
    public interface IJobResponsibilityTranslationRepository : IRepositoryAsync<JobResponsibilityTranslation> { }
    public interface IJobMainDutyRepository : IRepositoryAsync<JobMainDuty> { }
    public interface IJobMainDutyTranslationRepository : IRepositoryAsync<JobMainDutyTranslation> { }
    public interface IJobExtraBenefitRepository : IRepositoryAsync<JobExtraBenefit> { }
    public interface IJobExtraBenefitTranslationRepository : IRepositoryAsync<JobExtraBenefitTranslation> { }



}
