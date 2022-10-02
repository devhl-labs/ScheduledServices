using System;

namespace ScheduledServices.Services.Options
{
    public class ScheduledServiceOptions : ToggledServiceOptions, IScheduledServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }

        public ScheduledServiceOptions()
        {
        }

        public ScheduledServiceOptions(bool enabled, TimeSpan delayBeforeExecution, TimeSpan cancelAfter = default) : base(enabled, cancelAfter)
        {
            DelayBeforeExecution = delayBeforeExecution;
        }
    }
}
