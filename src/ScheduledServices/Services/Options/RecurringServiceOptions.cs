using System;

namespace ScheduledServices.Services.Options
{
    public class RecurringServiceOptions : ScheduledServiceOptions, IRecurringServiceOptions
    {
        public TimeSpan DelayBetweenExecutions { get; set; }
    }
}
