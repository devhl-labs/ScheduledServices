using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class DelayedService : ScheduledService
    {
        private readonly ILogger<DelayedService> _logger;

        public DelayedService(ILogger<DelayedService> logger, IOptions<DelayedServiceOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The delayed service is executing.");

            // any async work
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }

        protected override ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
        {
            // defaults to options.Value.DelayBeforeExecution
            return base.GetDelayBeforeExecutionAsync(cancellationToken);
        }
    }
}
