using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices
{
    public abstract class ToggledService : BackgroundService
    {
        internal readonly ILogger _logger;
        private readonly IOptions<IToggledServiceOptions> _options;


        public ToggledService(ILogger logger, IOptions<IToggledServiceOptions> options)
        {
            _logger = logger;
            _options = options;

            if (!_options.Value.Enabled)
                _logger.LogWarning("{0} Service is disabled!", GetType().FullName);
        }


        protected abstract Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken);

        protected virtual void OnExecutionError(Exception e, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                _logger.LogError(e, "An exception occured while executing recurring service {0}", GetType().FullName);
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

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_options.Value.Enabled)
                return;

            await SwallowAsync(ExecuteScheduledTaskAsync, cancellationToken);
        }
    }
}
