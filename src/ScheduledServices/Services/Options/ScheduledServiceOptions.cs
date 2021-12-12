using System;

namespace ScheduledServices.Services.Options
{
    public class ScheduledServiceOptions : IScheduledServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }
        public bool Enabled { get; set; }
    }
}
