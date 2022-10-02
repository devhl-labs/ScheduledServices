using System;

namespace ScheduledServices.Services.Options
{
    public class RecurringServiceOptions : ScheduledServiceOptions, IRecurringServiceOptions
    {
        public TimeSpan DelayBetweenExecutions { get; set; }

        public RecurringServiceOptions()
        {
        }

        public RecurringServiceOptions(bool enabled, TimeSpan delayBeforeExecution, TimeSpan delayBetweenExecutions, TimeSpan cancelAfter = default) : base (enabled, delayBeforeExecution, cancelAfter)
        {
            DelayBetweenExecutions = delayBetweenExecutions;
        }
    }
}
