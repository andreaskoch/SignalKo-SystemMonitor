namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemStorageDeviceInformation
    {
        public string DeviceName { get; set; }

        public double FreeDiscSpaceInPercent { get; set; }

        public override string ToString()
        {
            return string.Format("SystemStorageDeviceInformation (DeviceName: {0}, FreeDiscSpaceInPercent: {1})", this.DeviceName, this.FreeDiscSpaceInPercent);
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

            var otherObj = obj as SystemStorageDeviceInformation;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString());
            }

            return false;
        }
    }
}