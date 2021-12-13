using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class StartupWorker : ToggledService
    {
        private readonly ILogger<ScheduledWorker> _logger;

        public StartupWorker(ILogger<ScheduledWorker> logger, IOptions<StartupWorkerOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The startup service is executing.");

            // any async work
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
}
