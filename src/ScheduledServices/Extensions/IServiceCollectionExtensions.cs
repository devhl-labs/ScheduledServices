using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ScheduledServices.Extensions
{
    public static class IServiceCollectionExtensions
    {
#nullable disable
        internal static IConfiguration Configuration { get; set; }
#nullable enable

        /// <summary>
        /// Provides the IConfiguration to the ScheduledServices library.
        /// Not required if using ConfigureScheduledServices.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddScheduledServices(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;

            return services;
        }

        /// <summary>
        /// Configures your service to use a section from the IConfiguration.
        /// </summary>
        /// <typeparam name="TToggledService"></typeparam>
        /// <typeparam name="TToggledServiceOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceCollection AddScheduledService<TToggledService, TToggledServiceOptions>(
            this IServiceCollection services, string? path = "Services:")
                where TToggledService : ToggledService
                where TToggledServiceOptions : class, IToggledServiceOptions
        {
            if (Configuration == null)
                throw new NullReferenceException("IConfiguration must be provided before adding services. Use AddScheduledServices to provide it.");

            services.Configure<TToggledServiceOptions>(Configuration.GetCustomSection<TToggledService>(path));

            return services;
        }

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
    }
}
