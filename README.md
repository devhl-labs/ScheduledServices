# ScheduledServices
This is a simple library that makes it easy to schedule your services.
You can schedule a service to run after a delay, or to keep running with some delay between runs.

To implement this, have your service inherit ToggleService, ScheduledService, or RecurringService.
You can also override GetDelayBeforeExecutionAsync or GetDelayBetweenExecutionsAsync if you need to compute the delay.
The example below shows how to configure your service to run one minute after startup and keep running every minute.

## appsettings.json
```json
{
  "Services": {
    "YourService": {
      "Enabled": true,
      "DelayBeforeExecution": "00:01:00.0",
      "DelayBetweenExecutions": "00:01:00.0"
    }
}
```

## Program.cs
```cs
public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
    .ConfigureScheduledServices((host, scheduledServicesConfiguration) =>
    {
        scheduledServicesConfiguration.Configuration = host.Configuration;
        scheduledServicesConfiguration.Path = "Services:";
    })
    .ConfigureServices((context, services) =>
    {
        // schedules a service with no extension methods
        services.Configure<YourServiceOptions>(context.Configuration.GetSection($"Services:{typeof(YourService).Name}"));
            .AddSingleton<YourService>()
            .AddHostedService(services => services.GetRequiredService<YourService>());

        // schedules a service using provided extension methods
        services.ConfigureScheduledService<YourService, YourServiceOptions>().AddHostedSingleton<YourService>();
    });
```

## Options
```cs
public class YourServiceOptions : IRecurringServiceOptions
{
    // implement the interface, or inherit RecurringServiceOptions

    public TimeSpan DelayBeforeExecution { get; set; }
    public TimeSpan DelayBetweenExecutions { get; set; }
    public bool Enabled { get; set; }
}
```

## Service
```cs
public class YourService : RecurringService
{
    public YourService(ILogger<YourService> logger, IOptions<YourServiceOptions> options) : base(logger, options)
    {
    }

    protected override async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken)
    {
        // any async work
        // errors will be logged
    }

    protected override ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
    {
        // optionally you can compute the delay here
    }
}
```