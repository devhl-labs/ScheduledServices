using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices
{
    public abstract class ScheduledService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IOptions<IScheduledServiceOptions> _options;


        public ScheduledService(ILogger logger, IOptions<IScheduledServiceOptions> options)
        {
            _logger = logger;
            _options = options;
        }


        protected abstract Task DoWorkAsync(CancellationToken cancellationToken);

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

        protected virtual void OnExecutionError(Exception e, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                _logger.LogError(e, "An exception occured while executing recurring service {0}", GetType().FullName);
        }

        protected virtual void OnDelayError(Exception e, string name, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                _logger.LogError(e, "An exception occured while executing {0} in recurring service {1}", name, GetType().FullName);
        }

        internal protected async Task SwallowAsync<T>(Func<CancellationToken, T> func, CancellationToken cancellationToken) where T : Task
        {
            try
            {
                await func(cancellationToken);
            }
            catch (Exception e)
            {
                OnExecutionError(e, cancellationToken);
            }
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
                return;

            TimeSpan delay = await GetDelayOrDefaultAsync(GetDelayBeforeExecutionAsync, _options.Value.DelayBeforeExecution, cancellationToken);

            await DelayAsync(delay, cancellationToken);

            await SwallowAsync(DoWorkAsync, cancellationToken);
        }
    }
}
