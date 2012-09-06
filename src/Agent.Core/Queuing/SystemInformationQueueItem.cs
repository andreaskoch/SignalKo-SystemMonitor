using System;

using SignalKo.SystemMonitor.Common.Model;

namespace SignalKo.SystemMonitor.Agent.Core.Queuing
{
    public class SystemInformationQueueItem : IQueueItem<SystemInformation>
    {
        public SystemInformationQueueItem(SystemInformation item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.EnqueuCount = 0;
            this.Item = item;
        }

        public int EnqueuCount { get; set; }

        public SystemInformation Item { get; set; }
    }
}