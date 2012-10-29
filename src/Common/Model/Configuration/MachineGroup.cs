using System.Collections.Generic;

namespace SignalKo.SystemMonitor.Common.Model.Configuration
{
    /// <summary>
    /// Contains a collection of Machines and the configuration values.
    /// </summary>
    public class MachineGroup
    {
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string groupName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MachineGroup"/> is enabled for Monitoring.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool enabled { get; set; }

        /// <summary>
        /// Gets or sets the collection of the assigned machines.
        /// </summary>
        public Machine[] monitorMachines { get; set; }
    }
}