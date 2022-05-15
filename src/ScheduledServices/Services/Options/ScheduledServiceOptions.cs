using System;

namespace ScheduledServices.Services.Options
{
    public class ScheduledServiceOptions : ToggledServiceOptions, IScheduledServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }

        public ScheduledServiceOptions()
        {
        }

        public ScheduledServiceOptions(bool enabled, TimeSpan delayBeforeExecution) : base(enabled)
        {
            DelayBeforeExecution = delayBeforeExecution;
        }
    }
}
