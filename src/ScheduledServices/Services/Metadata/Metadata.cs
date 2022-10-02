using System;
using System.Runtime.ExceptionServices;

namespace ScheduledServices.Services.Metadata
{
    public class Metadata
    {
        /// <summary>
        /// The number of successful executions.
        /// </summary>
        public int Successes { get; internal set; }

        /// <summary>
        /// The number of failed executions.
        /// </summary>
        public int Failures { get; internal set; }

        /// <summary>
        /// The start time of the last execution attempt.
        /// </summary>
        public DateTime Start { get; internal set; }

        /// <summary>
        /// The stop time of the last execution.
        /// </summary>
        public DateTime Stop { get; internal set; }

        /// <summary>
        /// The exception that occured during the last failure.
        /// </summary>
        public Exception? Exception { get; internal set; }

        /// <summary>
        /// The DateTime the exception occured.
        /// </summary>
        public DateTime? ExceptionAt { get; internal set;}
    }
}
