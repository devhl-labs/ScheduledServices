using System;
using ScheduledServices.Services.Options;

namespace ScheduledServices
{
    public interface IScheduledServiceOptions : IToggledServiceOptions
    {
        TimeSpan DelayBeforeExecution { get; set; }
    }
}
