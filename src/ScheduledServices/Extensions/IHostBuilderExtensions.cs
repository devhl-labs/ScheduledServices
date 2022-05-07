using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ScheduledServices.Extensions
{
    public static class IHostBuilderExtensions
    {
        /// <summary>
        /// Configures the ScheduledServices
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="builderContext"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureScheduledServices(this IHostBuilder builder, Action<HostBuilderContext, IServiceCollection> builderContext)
        {
            builder.ConfigureServices((context, services) =>
            {
                IServiceCollectionExtensions.Configuration = context.Configuration;

                builderContext(context, services);
            });

            return builder;
        }
    }
}
