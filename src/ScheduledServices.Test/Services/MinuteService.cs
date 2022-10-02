using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class MinuteService : RecurringService
    {
        private readonly ILogger<MinuteService> _logger;

        public MinuteService(ILogger<MinuteService> logger, IOptions<MinuteServiceOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The minute service is executing at {dateTime}.", DateTime.Now);

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
