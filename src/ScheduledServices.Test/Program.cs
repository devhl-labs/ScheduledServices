using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ScheduledServices.Test
{
    public class Program
    {
        const string PATH = "Services:";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureScheduledServices((host, scheduledServicesConfiguration) =>
                {
                    scheduledServicesConfiguration.Configuration = host.Configuration;
                    scheduledServicesConfiguration.Path = PATH;
                })
                .ConfigureServices((context, services) =>
                {
                    // scheduling a service without any extension methods
                    services.Configure<DelayedServiceOptions>(context.Configuration.GetRequiredSection($"{PATH}{typeof(DelayedService).Name}"))
                        .AddHostedService<DelayedService>();

                    // use GetRequiredSection<T> to get Services:MinuteService
                    services.Configure<MidnightServiceOptions>(context.Configuration.GetRequiredSection<MidnightService>("Services:")).AddHostedService<MidnightService>();

                    // use GetServicesSection to get Services:MinuteService
                    services.Configure<MinuteServiceOptions>(context.Configuration.GetServicesSection<MinuteService>()).AddHostedService<MinuteService>();

                    // schedule services using ConfigureScheduledService<TService, TServiceOptions>
                    services.ConfigureScheduledService<StartupService, StartupServiceOptions>().AddHostedService<StartupService>();
                });
    }
}
