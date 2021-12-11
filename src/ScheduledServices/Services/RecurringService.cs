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

            TimeSpan? delay = await GetDelayOrDefaultAsync(GetDelayBeforeExecutionAsync, _options.Value.DelayBeforeExecution, cancellationToken);

            if (delay == null)
                return;

            await DelayAsync(delay.Value, cancellationToken);

            while (_options.Value.Enabled && !cancellationToken.IsCancellationRequested)
            {
                await SwallowAsync(DoWorkAsync, cancellationToken);

                delay = await GetDelayOrDefaultAsync(GetDelayBetweenExecutionsAsync, _options.Value.DelayBetweenExecutions, cancellationToken);

                if (delay == null)
                    continue;

                await DelayAsync(delay.Value, cancellationToken);
            }
        }
    }
}
