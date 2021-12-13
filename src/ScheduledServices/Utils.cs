using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduledServices;

namespace ScheduledBackgroundServices
{
    public static class Utils
    {
        /// <summary>
        /// Adds a hosted service as a singleton.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHostedSingleton<TService>(this IServiceCollection services)
            where TService : ToggledService
        {
            services.AddSingleton<TService>();

            services.AddHostedService(services => services.GetRequiredService<TService>());

            return services;
        }

        /// <summary>
        /// Adds a hosted service as a singleton.
        /// Configures your TService to use TOptions.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddHostedSingleton<TService, TOptions>(this IServiceCollection services, IConfigurationSection config)
            where TService : ToggledService
            where TOptions : class, IToggledServiceOptions
        {
            services.Configure<TOptions>(config);

            return AddHostedSingleton<TService>(services);
        }

        /// <summary>
        /// Returns the configuration section from the appsettings for your service.
        /// Path defaults to Services:{typeof(TOptions).Name}
        /// </summary>
        /// <param name="context"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IConfigurationSection GetSection(this HostBuilderContext context, string path)
            => context.Configuration.GetSection(path);

        /// <summary>
        /// Returns the configuration section from the appsettings for your service using appsettings path $"Services:{typeof(TService).Name}"
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IConfigurationSection GetSection<TService>(this HostBuilderContext context) where TService : ToggledService
            => context.Configuration.GetSection($"Services:{typeof(TService).Name}");
    }
}
