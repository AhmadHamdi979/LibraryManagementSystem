using Autofac.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Library.Application.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Library.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
      
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

            services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            ValidatorOptions.Global.LanguageManager.Enabled = true;


            return services;
        }
    }
}
