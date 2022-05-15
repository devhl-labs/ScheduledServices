namespace ScheduledServices.Services.Options
{
    public class ToggledServiceOptions : IToggledServiceOptions
    {
        public bool Enabled { get; set; }

        public ToggledServiceOptions()
        {
        }

        public ToggledServiceOptions(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
