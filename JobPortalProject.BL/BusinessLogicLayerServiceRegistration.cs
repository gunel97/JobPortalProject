using JobPortalProject.BL.Mapping;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.Services.Implementations;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.UI.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped<IJobCategoryService, JobCategoryManager>();
            services.AddScoped<IJobCategoryTranslationService, JobCategoryTranslationManager>();

            services.AddScoped<IHomeService, HomeManager>();
            services.AddScoped<ITopHeaderService, TopHeaderManager>();

            services.AddScoped<ICloudinaryService, CloudinaryManager>();
            services.AddScoped<ICookieService, CookieManager>();
            services.AddSingleton<StringLocalizerManager>();

            return services;
        }
    }
}
