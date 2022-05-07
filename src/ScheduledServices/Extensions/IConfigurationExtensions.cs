using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScheduledServices.Extensions
{
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Returns the configuration section from the appsettings for your service using appsettings path $"{path}{typeof(TService).Name}"
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetCustomSection<TService>(this IConfiguration config, string? path = null)
            => config.GetRequiredSection($"{path}{typeof(TService).Name}");

        /// <summary>
        /// Returns the configuration section at Services:YourService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetServicesSection<TService>(this IConfiguration config)
            => config.GetCustomSection<TService>("Services:");

        /// <summary>
        /// Returns the configuration section at Options:YourService
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IConfigurationSection GetOptionsSection<TService>(this IConfiguration config)
            => config.GetCustomSection<TService>("Options:");
    }
}
