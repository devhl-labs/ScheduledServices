using Microsoft.Extensions.Configuration;

namespace ScheduledServices
{
    public class ScheduledServicesConfiguration
    {
        public IConfiguration? Configuration { get; set; }

        /// <summary>
        /// The path to use to find your service configurations.
        /// </summary>
        public string? Path { get; set; }
    }
}
