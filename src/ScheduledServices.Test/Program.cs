using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduledServices;

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
                    services.Configure<ScheduledWorkerOptions>(context.Configuration.GetSection($"Services:{typeof(ScheduledWorker).Name}"))
                        .AddHostedService<ScheduledWorker>();

                    services.Configure<RecurringWorkerOptions>(context.Configuration.GetSection($"Services:{typeof(RecurringWorker).Name}"))
                        .AddHostedService<RecurringWorker>();

                    services.Configure<MidnightWorkerOptions>(context.Configuration.GetSection<MidnightWorker>("Services:"))
                        .AddHostedService<MidnightWorker>();

                    services.Configure<StartupWorkerOptions>(context.Configuration.GetSection<StartupWorker>())
                        .AddHostedService<StartupWorker>();
                });
    }
}
