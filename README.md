# ScheduledServices
This is a simple library that makes it easy to manage the lifetime of your services.
You can schedule a service to run after some delay, or to keep running with some delay between runs.

To implement this, have your service inherit ToggleService, ScheduledService, or RecurringService.
Be sure to pass in an IOptions\<IToggledServiceOptions\>, IOptions\<IScheduledServiceOptions\> or IOptions\<IRecurringServiceOptions\>.
You can also override GetDelayBeforeExecutionAsync or GetDelayBetweenExecutionsAsync if you need to compute the delay.
The example below shows how to configure your service to run one minute after startup.

## appsettings.json
```json
{
  "Services": {
    "YourService": {
      "Enabled": true,
      "DelayBeforeExecution": "00:01:00.0",
      "DelayBetweenExecutions": "00:00:00.0"
    }
}
```

## Program.cs
```cs
public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<YourServiceOptions>(context.Configuration.GetSection($"Services:{typeof(YourService).Name}"));
        services.AddSingleton<YourService>();
        services.AddHostedService(services => services.GetRequiredService<YourService>());

        // or do it in one command
        // assumes the appsettings section is at Settings:YourService
        services.AddHostedSingleton<YourService, YourServiceOptions>(context.GetSection<YourService>());
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

    protected override async Task ExecuteScheduledWorkAsync(CancellationToken cancellationToken)
    {
        // any async work
        // errors will be logged and caught
    }

    protected override ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
    {
        // optionally you can compute the delay here
    }
}
```