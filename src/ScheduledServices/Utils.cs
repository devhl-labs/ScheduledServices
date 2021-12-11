using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScheduledBackgroundServices
{
    public static class Utils
    {
        public static IServiceCollection AddHostedSingletonService<TType>(this IServiceCollection services)
            where TType : class, IHostedService
        {
            services.AddSingleton<TType>();

            services.AddHostedService(services => services.GetRequiredService<TType>());

            return services;
        }
    }
}
