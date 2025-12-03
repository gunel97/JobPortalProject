using JobPortalProject.BL.Mapping;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.Services.Implementations;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
namespace JobPortalProject.BL
{
    public static class BusinessLogicLayerServiceRegistration
    {
        public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(
               options =>
               {
                   var supportedCultures = new List<CultureInfo>
                       {
                        new CultureInfo("en-US"),
                        new CultureInfo("az"),
                        new CultureInfo("de")
                       };

                   options.DefaultRequestCulture = new RequestCulture(culture: "az", uiCulture: "az");

                   options.SupportedCultures = supportedCultures;
                   options.SupportedUICultures = supportedCultures;

               });

            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

            services.AddScoped(typeof(ICrudService<,,,>), typeof(CrudManager<,,,>));

            services.AddScoped<ILanguageService, LanguageManager>();
            services.AddScoped<ICountryTranslationService, CountryTranslationManager>();
            services.AddScoped<ICityTranslationService, CityTranslationManager>();
            services.AddScoped<IAddressTranslationService, AddressTranslationManager>();
            services.AddScoped<ICountryService, CountryManager>();
            services.AddScoped<ICityService, CityManager>();
            services.AddScoped<IAddressService, AddressManager>();

            services.AddScoped<ICompanyService, CompanyManager>();
            services.AddScoped<ICompanyTranslationService, CompanyTranslationManager>();
            services.AddScoped<ICompanyTypeService, CompanyTypeManager>();
            services.AddScoped<ICompanyTypeTranslationService, CompanyTypeTranslationManager>();
            services.AddScoped<IWorkingFieldTranslationService, WorkingFieldTranslationManager>();
            services.AddScoped<IWorkingFieldService, WorkingFieldManager>();
            services.AddScoped<ISocialMediaService,  SocialMediaManager>();
            services.AddScoped<ICompanySocialService,  CompanySocialManager>();

            services.AddScoped<IJobCategoryService, JobCategoryManager>();
            services.AddScoped<IJobCategoryTranslationService, JobCategoryTranslationManager>();
            services.AddScoped<IJobTranslationService, JobTranslationManager>();
            services.AddScoped<IJobService, JobManager>();
            services.AddScoped<IJobListingService, JobListingManager>();

            services.AddScoped<ITopHeaderService, TopHeaderManager>();
            services.AddScoped<IHomeService, HomeManager>();
            services.AddScoped<ICompanyDetailsService, CompanyDetailsManager>();
            services.AddScoped<ICompanyListingService, CompanyListingManager>();
            services.AddScoped<ICompanyDashboardService, CompanyDashboardManager>();

            services.AddScoped<ICloudinaryService, CloudinaryManager>();
            services.AddScoped<ICookieService, CookieManager>();
            services.AddSingleton<StringLocalizerManager>();
            services.AddScoped<IUserService, UserManager>();

            return services;
        }
    }
}
