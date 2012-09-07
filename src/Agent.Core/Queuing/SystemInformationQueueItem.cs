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

        public SystemInformation Item { get; private set; }

        public int EnqueuCount { get; set; }

        public override string ToString()
        {
            return string.Format("SystemInformationQueueItem (Item: {0})", this.Item);
        }

        public override int GetHashCode()
        {
            int hash = 37;
            hash = (hash * 23) + this.ToString().GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var otherObj = obj as SystemInformationQueueItem;
            if (otherObj != null)
            {
                return this.Item.Equals(otherObj.Item);
            }

            return false;
        }
    }
}