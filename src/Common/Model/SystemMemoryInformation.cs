namespace SignalKo.SystemMonitor.Common.Model
{
    public class SystemMemoryInformation
    {
        public double AvailableMemoryInGB { get; set; }

        public double UsedMemoryInGB { get; set; }

        public override string ToString()
        {
            return string.Format("SystemMemoryInformation (AvailableMemoryInGB: {0}, UsedMemoryInGB: {1})", this.AvailableMemoryInGB, this.UsedMemoryInGB);
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

            var otherObj = obj as SystemMemoryInformation;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString());
            }

            return false;
        }
    }
}