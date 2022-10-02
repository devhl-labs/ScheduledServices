using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScheduledServices.Services.Metadata;

namespace ScheduledServices
{
    public abstract class ToggledService : BackgroundService
    {
        internal readonly ILogger _logger;
        private readonly IOptions<IToggledServiceOptions> _options;

        public Metadata Metadata { get; } = new();

        public ToggledService(ILogger logger, IOptions<IToggledServiceOptions> options)
        {
            _logger = logger;
            _options = options;

            if (!_options.Value.Enabled)
                _logger.LogWarning("{name} Service is disabled!", GetType().Name);
        }


        protected abstract Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken);

        protected virtual void OnExecutionError(Exception e, CancellationToken cancellationToken)
        {
            _logger.LogError(e, "An exception occured while executing service {name}.", GetType().Name);
        }

        private protected async Task SwallowAsync<T>(Func<CancellationToken, T> func, CancellationToken cancellationToken) where T : Task
        {
            Metadata.Start = DateTime.UtcNow;

            try
            {
                await func(cancellationToken);
                Metadata.Successes++;
            }
            catch (Exception e)
            {
                Metadata.Failures++;
                Metadata.Exception = e;
                Metadata.ExceptionAt = DateTime.UtcNow;
                OnExecutionError(e, cancellationToken);
            }

            Metadata.Stop = DateTime.UtcNow;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_options.Value.Enabled || cancellationToken.IsCancellationRequested)
                return;

            await ExecuteScheduledTaskWithTimeoutAsync(cancellationToken);
        }

        internal async Task ExecuteScheduledTaskWithTimeoutAsync(CancellationToken cancellationToken)
        {
            using var timeOutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            if (_options.Value.CancelAfter > TimeSpan.Zero)
                timeOutTokenSource.CancelAfter(_options.Value.CancelAfter);

            await SwallowAsync(ExecuteScheduledTaskAsync, timeOutTokenSource.Token);
        }

        protected virtual void AfterStopped()
        {
            _logger.LogInformation("{event} {name}", "Stopped", GetType().Name);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            AfterStopped();
        }

        protected virtual void AfterStarted()
        {
            _logger.LogInformation("{event} {name}", "Started", GetType().Name);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            AfterStarted();
        }
    }
}
