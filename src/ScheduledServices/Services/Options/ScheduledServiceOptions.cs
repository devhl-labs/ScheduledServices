using System;

namespace ScheduledServices.Services.Options
{
    public class ScheduledServiceOptions : ToggledServiceOptions, IScheduledServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }
    }
}
