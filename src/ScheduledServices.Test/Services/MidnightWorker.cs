using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class MidnightWorker : ScheduledService
    {
        private readonly ILogger<ScheduledWorker> _logger;

        public MidnightWorker(ILogger<ScheduledWorker> logger, IOptions<ScheduledWorkerOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The midnight service is executing.");

            // any async work
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }

        protected override ValueTask<TimeSpan> GetDelayBeforeExecutionAsync(CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime tomorrow = now.AddDays(1);
            DateTime midnight = new(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 0, 0);
            return new(midnight - now);
        }
    }
}
