using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA
{
    public static class DataAccessLayerServiceRegistration
    {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default"), options =>
                {
                    options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                }));

            services.AddScoped<DataInitializer>();

            services.AddScoped(typeof(IRepositoryAsync<>), typeof(EfCoreRepositoryAsync<>));
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IJobCategoryTranslationRepository, JobCategoryTranslationRepository>();
            services.AddScoped<ICountryTranslationRepository, CountryTranslationRepository>();
            services.AddScoped<ICityTranslationRepository, CityTranslationRepository>();
            services.AddScoped<IAddressTranslationRepository, AddressTranslationRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICompanyTypeRepository, CompanyTypeRepository>();
            services.AddScoped<ICompanyTypeTranslationRepository, CompanyTypeTranslationRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyTranslationRepository, CompanyTranslationRepository>();
            services.AddScoped<IWorkingFieldRepository, WorkingFieldRepository>();
            services.AddScoped<IWorkingFieldTranslationRepository, WorkingFieldTranslationRepository>();
            services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();
            services.AddScoped<ICompanySocialRepository, CompanySocialRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IJobTranslationRepository, JobTranslationRepository>();
            services.AddScoped<IJobResponsibilityRepository, JobResponsibilityRepository>();
            services.AddScoped<IJobResponsibilityTranslationRepository, JobResponsibilityTranslationRepository>();
            services.AddScoped<IJobMainDutyRepository, JobMainDutyRepository>();
            services.AddScoped<IJobMainDutyTranslationRepository, JobMainDutyTranslationRepository>();
            services.AddScoped<IJobExtraBenefitRepository, JobExtraBenefitRepository>();
            services.AddScoped<IJobExtraBenefitTranslationRepository, JobExtraBenefitTranslationRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
            services.AddScoped<IResumeTranslationRepository, ResumeTranslationRepository>();
            services.AddScoped<IPersonalInfoRepository, PersonalInfoRepository>();
            services.AddScoped<IPersonalInfoTranslationRepository, PersonalInfoTranslationRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IEducationTranslationRepository, EducationTranslationRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<IExperienceTranslationRepository, ExperienceTranslationRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();


            return services;
        }
    }
}
