using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduledBackgroundServices;
using ScheduledServices.Services.Options;

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
                .ConfigureServices((context, services) =>
                {
                    services.Configure<ScheduledWorkerOptions>(context.Configuration.GetSection($"Services:{typeof(ScheduledWorker).Name}"));
                    services.AddHostedService<ScheduledWorker>();

                    services.Configure<RecurringWorkerOptions>(context.Configuration.GetSection($"Services:{typeof(RecurringWorker).Name}"));
                    services.AddHostedSingleton<RecurringWorker>();

                    services.Configure<ScheduledWorkerOptions>(context.Configuration.GetSection($"Services:{typeof(MidnightWorker).Name}"));
                    services.AddHostedSingleton<MidnightWorker>();

                    // optionally configure a hosted service as a singleton while using options from Services:MidnightWorker
                    services.AddHostedSingleton<MidnightWorker, MidnightWorkerOptions>(context.GetSection<MidnightWorker>());                    
                });
    }
}
