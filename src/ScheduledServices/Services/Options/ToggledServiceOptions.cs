using System;
using System.Collections.Immutable;

namespace ScheduledServices.Services.Options
{
    public class ToggledServiceOptions : IToggledServiceOptions
    {
        public bool Enabled { get; set; }
    }
}
