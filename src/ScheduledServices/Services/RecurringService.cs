using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices
{
    public abstract class RecurringService : ScheduledService
    {
        private readonly IOptions<IRecurringServiceOptions> _options;
        private bool _isFirstRun = true;

        public RecurringService(ILogger logger, IOptions<IRecurringServiceOptions> options) : base(logger, options)
        {
            _options = options;
        }

        protected virtual ValueTask<TimeSpan> GetDelayBetweenExecutionsAsync(CancellationToken cancellationToken)
            => new(_options.Value.DelayBetweenExecutions);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (_options.Value.Enabled && !cancellationToken.IsCancellationRequested)
            {
                if (_isFirstRun)
                {
                    TimeSpan delayBefore = await GetDelayOrDefaultAsync(GetDelayBeforeExecutionAsync, _options.Value.DelayBeforeExecution, cancellationToken);

                    await DelayAsync(delayBefore, cancellationToken);

                    _isFirstRun = true;
                }

                await ExecuteScheduledTaskWithTimeoutAsync(cancellationToken);

                TimeSpan delayBetween = await GetDelayOrDefaultAsync(GetDelayBetweenExecutionsAsync, _options.Value.DelayBetweenExecutions, cancellationToken);

                await DelayAsync(delayBetween, cancellationToken);
            }
        }
    }
}
