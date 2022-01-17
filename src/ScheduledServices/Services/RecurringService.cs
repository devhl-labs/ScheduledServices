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

        public RecurringService(ILogger logger, IOptions<IRecurringServiceOptions> options) : base(logger, options)
        {
            _options = options;
        }

        protected virtual ValueTask<TimeSpan> GetDelayBetweenExecutionsAsync(CancellationToken cancellationToken)
            => new(_options.Value.DelayBetweenExecutions);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_options.Value.Enabled)
                return;

            TimeSpan delay = await GetDelayOrDefaultAsync(GetDelayBeforeExecutionAsync, _options.Value.DelayBeforeExecution, cancellationToken);

            await DelayAsync(delay, cancellationToken);

            while (_options.Value.Enabled && !cancellationToken.IsCancellationRequested)
            {
                await SwallowAsync(ExecuteScheduledTaskAsync, cancellationToken);

                delay = await GetDelayOrDefaultAsync(GetDelayBetweenExecutionsAsync, _options.Value.DelayBetweenExecutions, cancellationToken);

                await DelayAsync(delay, cancellationToken);
            }
        }
    }
}
