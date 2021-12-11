using System;

namespace ScheduledServices.Test
{
    public class ScheduledWorkerOptions : IScheduledServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }
        public bool Enabled { get; set; }
    }
}
