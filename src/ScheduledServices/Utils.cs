using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScheduledServices
{
    public static class Utils
    {
        /// <summary>
        /// Adds singleton and registers that instance as a hosted service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHostedSingleton<TService>(this IServiceCollection services)
            where TService : class, IHostedService
        {
            services.AddSingleton<TService>();

            services.AddHostedService(services => services.GetRequiredService<TService>());

            return services;
        }

        /// <summary>
        /// Returns the configuration section from the appsettings for your service using appsettings path $"Services:{typeof(TService).Name}"
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetSection<TService>(this IConfiguration config, string pathPrefix = "Services:")
            => config.GetSection($"{pathPrefix}{typeof(TService).Name}");
    }
}
