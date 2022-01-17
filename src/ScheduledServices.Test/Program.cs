using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace ScheduledServices.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureScheduledServices((host, scheduledServicesConfiguration) =>
                {
                    scheduledServicesConfiguration.Configuration = host.Configuration;
                    scheduledServicesConfiguration.Path = "Services:";
                })
                .ConfigureServices((context, services) =>
                {
                    // scheduling a service without any extension methods
                    services.Configure<DelayedServiceOptions>(context.Configuration.GetRequiredSection($"Services:{typeof(DelayedService).Name}"))
                        .AddHostedService<DelayedService>();

                    // schedule services using provided extension method
                    services.ConfigureScheduledService<MinuteService, MinuteServiceOptions>().AddHostedService<MinuteService>();
                    services.ConfigureScheduledService<MidnightService, MidnightServiceOptions>().AddHostedService<MidnightService>();
                    services.ConfigureScheduledService<StartupService, StartupServiceOptions>().AddHostedService<StartupService>();
                });
    }
}
