using System;

namespace ScheduledServices
{
    public interface IScheduledServiceOptions
    {
        TimeSpan DelayBeforeExecution { get; set; }
        bool Enabled { get; set; }
    }
}
