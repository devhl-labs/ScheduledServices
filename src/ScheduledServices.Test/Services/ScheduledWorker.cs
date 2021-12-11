using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class ScheduledWorker : ScheduledService
    {
        private readonly ILogger<ScheduledWorker> _logger;

        public ScheduledWorker(ILogger<ScheduledWorker> logger, IOptions<ScheduledWorkerOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The scheduled service is executing.");

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
