using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScheduledServices
{
    public static class Utils
    {
        private static IConfiguration? _configuration;
        private static string? _path;

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
        /// Returns the configuration section from the appsettings for your service using appsettings path $"{path}{typeof(TService).Name}"
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetRequiredSection<TService>(this IConfiguration config, string? path = null) => config.GetRequiredSection($"{path}{typeof(TService).Name}");

        /// <summary>
        /// Returns the configuration section from the appsettings for your service.
        /// Assumes the path is the value provided in ConfigureScheduledServices or AddScheduledServices
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetServicesSection<TService>(this IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(_path))
                throw new Exception("The path must be provided either in the method or when configuring ScheduledServices.");

            return config.GetRequiredSection<TService>(_path);
        }

        /// <summary>
        /// Configures your service to use a section from the IConfiguration.
        /// </summary>
        /// <typeparam name="TToggledService"></typeparam>
        /// <typeparam name="TToggledServiceOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureScheduledService<TToggledService, TToggledServiceOptions>
        (
            this IServiceCollection services, IConfiguration configuration, string? path
        )
            where TToggledService : ToggledService
            where TToggledServiceOptions : class, IToggledServiceOptions
        {
            if (configuration == null)
                throw new Exception("IConfiguration was not provided. Provide it by configuring the ScheduledServices library or with the method overload.");

            services.Configure<TToggledServiceOptions>(configuration.GetRequiredSection<TToggledService>(path));

            return services;
        }

        /// <summary>
        /// Configures your service to use a section from the IConfiguration.
        /// </summary>
        /// <typeparam name="TToggledService"></typeparam>
        /// <typeparam name="TToggledServiceOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureScheduledService<TToggledService, TToggledServiceOptions>
        (
            this IServiceCollection services, IConfiguration configuration
        )
            where TToggledService : ToggledService
            where TToggledServiceOptions : class, IToggledServiceOptions
        {
            services.Configure<TToggledServiceOptions>(configuration.GetServicesSection<TToggledService>());

            return services;
        }

        /// <summary>
        /// Configures your service to use a section from the IConfiguration.
        /// </summary>
        /// <typeparam name="TToggledService"></typeparam>
        /// <typeparam name="TToggledServiceOptions"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection ConfigureScheduledService<TToggledService, TToggledServiceOptions>(this IServiceCollection services)
                where TToggledService : ToggledService
                where TToggledServiceOptions : class, IToggledServiceOptions
            => ConfigureScheduledService<TToggledService, TToggledServiceOptions>(services, _configuration!, _path);
        

        private static void AddScheduledServices(ScheduledServicesConfiguration scheduledServicesConfiguration)
        {
            _configuration = scheduledServicesConfiguration.Configuration;
            _path = scheduledServicesConfiguration.Path;
        }

        /// <summary>
        /// Configures your service to use a section from the IConfiguration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="scheduledServicesConfigurationContext"></param>
        public static void AddScheduledServices(this IServiceCollection services, Action<ScheduledServicesConfiguration> scheduledServicesConfigurationContext)
        {
            ScheduledServicesConfiguration scheduledServicesConfiguration = new();

            scheduledServicesConfigurationContext(scheduledServicesConfiguration);

            AddScheduledServices(scheduledServicesConfiguration);
        }

        /// <summary>
        /// Configures the ScheduledServices library. When configured, you can configure the services without providing any paramters.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="scheduledServicesConfigurationContext"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureScheduledServices(this IHostBuilder builder, Action<HostBuilderContext, ScheduledServicesConfiguration> scheduledServicesConfigurationContext)
        {
            builder.ConfigureServices((context, services) =>
            {
                ScheduledServicesConfiguration scheduledServicesConfiguration = new();

                scheduledServicesConfigurationContext(context, scheduledServicesConfiguration);

                AddScheduledServices(scheduledServicesConfiguration);
            });

            return builder;
        }
    }
}
