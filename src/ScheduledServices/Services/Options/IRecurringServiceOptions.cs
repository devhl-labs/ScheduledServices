using System;

namespace ScheduledServices
{
    public interface IRecurringServiceOptions : IScheduledServiceOptions
    {
        TimeSpan DelayBetweenExecutions { get; set; }
    }
}
