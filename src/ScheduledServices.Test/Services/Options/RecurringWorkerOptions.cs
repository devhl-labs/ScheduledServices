using System;

namespace ScheduledServices.Test
{
    public class RecurringWorkerOptions : IRecurringServiceOptions
    {
        public TimeSpan DelayBeforeExecution { get; set; }
        public TimeSpan DelayBetweenExecutions { get; set; }
        public bool Enabled { get; set; }
    }
}
