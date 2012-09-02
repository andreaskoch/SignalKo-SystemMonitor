using System.Linq;

namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemStorageInformation
    {
        public SystemStorageDeviceInformation[] StorageDeviceInfos { get; set; }

        public override string ToString()
        {
            return this.StorageDeviceInfos != null
                       ? string.Format("SystemStorageInformation ({0})", string.Join(", ", this.StorageDeviceInfos.Select(s => s.ToString())))
                       : "SystemStorageInformation (empty)";
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

            var otherObj = obj as SystemStorageInformation;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString());
            }

            return false;
        }
    }
}