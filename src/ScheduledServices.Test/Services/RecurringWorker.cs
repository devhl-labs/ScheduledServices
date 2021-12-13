using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class RecurringWorker : RecurringService
    {
        private readonly ILogger<RecurringWorker> _logger;

        public RecurringWorker(ILogger<RecurringWorker> logger, IOptions<RecurringWorkerOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The recurring service is executing.");

            // any async work
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }

        protected override ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
        {
            // defaults to options.Value.DelayBeforeExecution
            return base.GetDelayBeforeExecutionAsync(cancellationToken);
        }

        protected override ValueTask<TimeSpan> GetDelayBetweenExecutionsAsync(CancellationToken cancellationToken)
        {
            // defaults to options.Value.DelayBetweenExecutions
            return base.GetDelayBetweenExecutionsAsync(cancellationToken);
        }
    }
}
