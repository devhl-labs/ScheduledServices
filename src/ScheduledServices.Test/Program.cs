using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ScheduledServices.Extensions;
using System;

namespace ScheduledServices.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine($"Started at {DateTime.Now}.");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureScheduledServices((context, services) =>
                {
                    // scheduling a service without any extension methods
                    services.Configure<DelayedServiceOptions>(context.Configuration.GetRequiredSection($"Services:{typeof(DelayedService).Name}"))
                        .AddHostedService<DelayedService>();

                    // use GetRequiredSection<T> to configure your service with a custom path such as CustomPath:MidnightService
                    services.Configure<MidnightServiceOptions>(context.Configuration.GetCustomSection<MidnightService>("Services:"))
                        .AddHostedService<MidnightService>();

                    // use GetServicesSection<T> to configure your service using Services:MinuteService
                    services.Configure<MinuteServiceOptions>(context.Configuration.GetServicesSection<MinuteService>())
                        .AddHostedService<MinuteService>();

                    // schedule your service using Services:StartupService
                    services.ConfigureScheduledService<StartupService, StartupServiceOptions>()
                        .AddHostedService<StartupService>();
                });
    }
}
