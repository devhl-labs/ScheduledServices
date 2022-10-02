using System;

namespace ScheduledServices.Services.Options
{
    public class ToggledServiceOptions : IToggledServiceOptions
    {
        public bool Enabled { get; set; }

        public TimeSpan CancelAfter { get; set; }

        public ToggledServiceOptions()
        {
        }

        public ToggledServiceOptions(bool enabled, TimeSpan cancelAfter = default)
        {
            Enabled = enabled;
            CancelAfter = cancelAfter;
        }
    }
}
