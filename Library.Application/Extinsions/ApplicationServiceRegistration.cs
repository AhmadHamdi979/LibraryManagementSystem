using Library.Application.Mappings;
using Microsoft.Extensions.DependencyInjection; 

namespace Library.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
      
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

            return services;
        }
    }
}
