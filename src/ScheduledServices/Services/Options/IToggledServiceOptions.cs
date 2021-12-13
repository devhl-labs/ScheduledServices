using System;

namespace ScheduledServices
{
    public interface IToggledServiceOptions
    {
        bool Enabled { get; set; }
    }
}
