namespace SignalKo.SystemMonitor.Common.Model.Configuration
{
    public class Machine
    {
        /// <summary>
        /// Gets or sets the url to the Health Check Monitor.
        /// </summary>
        public string healthCheckMonitorUrl { get; set; }


        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public string machineName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is webserver.
        /// </summary>
        /// <value><c>true</c> if this instance is webserver; otherwise, <c>a Database server</c>.</value>
        public bool isWebserver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Monitor Agent of this Machine.
        /// </summary>
        /// <value><c>true</c> if the Monitor Agent is enabled; otherwise, <c>false</c>.</value>
        public bool monitorAgentEnabled { get; set; }
    }
}