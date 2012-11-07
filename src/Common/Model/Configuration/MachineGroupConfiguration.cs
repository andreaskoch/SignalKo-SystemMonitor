using System.Collections.Generic;

namespace SignalKo.SystemMonitor.Common.Model.Configuration
{
    public class MachineGroupConfiguration
    {
        /// <summary>
        /// Gets or sets the collection of the not to a group assigned machines.
        /// </summary>
        public Machine[] availableMachines { get; set; }

        /// <summary>
        /// Gets or sets the collection of machine groups.
        /// </summary>
        public MachineGroup[] machineGroups { get; set; }
    }
}