using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScheduledServices.Test
{
    public class MidnightService : ScheduledService
    {
        private readonly ILogger<MidnightService> _logger;

        public MidnightService(ILogger<MidnightService> logger, IOptions<MidnightServiceOptions> options) : base(logger, options)
        {
            _logger = logger;
        }

        protected override async Task ExecuteScheduledTaskAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The midnight service is executing at {dateTime}.", DateTime.Now);

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
