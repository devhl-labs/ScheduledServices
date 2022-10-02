using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class StartupService : ToggledService
    {
        private readonly ILogger<StartupService> _logger;

        public StartupService(ILogger<StartupService> logger, IOptions<StartupServiceOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The startup service is executing at {dateTime}.", DateTime.Now);

            // any async work
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}
