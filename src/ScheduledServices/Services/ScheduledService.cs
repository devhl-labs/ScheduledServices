using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices
{
    public abstract class ScheduledService : ToggledService
    {
        private readonly IOptions<IScheduledServiceOptions> _options;


        public ScheduledService(ILogger logger, IOptions<IScheduledServiceOptions> options) : base(logger, options)
        {
            _options = options;
        }


        protected virtual ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
            => new(_options.Value.DelayBeforeExecution);

        internal protected async ValueTask<TimeSpan> GetDelayOrDefaultAsync(Func<CancellationToken, ValueTask<TimeSpan>> func, TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            try
            {
                return await func(cancellationToken);
            }
            catch (Exception e)
            {
                OnDelayError(e, func.Method.Name, cancellationToken);
            }

            return timeSpan;
        }

        protected virtual void OnDelayError(Exception e, string name, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                _logger.LogError(e, "An exception occured while executing {0} in recurring service {1}", name, GetType().FullName);
        }

        internal protected static async Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch (Exception)
            {
            }
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_options.Value.Enabled)
            {
                _logger.LogWarning("{0} Service is disabled!", GetType().FullName);
                return;
            }

            TimeSpan delay = await GetDelayOrDefaultAsync(GetDelayBeforeExecutionAsync, _options.Value.DelayBeforeExecution, cancellationToken);

            await DelayAsync(delay, cancellationToken);

            await SwallowAsync(ExecuteScheduledWorkAsync, cancellationToken);
        }
    }
}
