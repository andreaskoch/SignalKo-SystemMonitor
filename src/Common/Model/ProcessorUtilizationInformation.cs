namespace SignalKo.SystemMonitor.Common.Model
{
    public class ProcessorUtilizationInformation
    {
        public double ProcessorUtilizationInPercent { get; set; }

        public override string ToString()
        {
            return string.Format("ProcessorUtilizationInformation (ProcessorUtilizationInPercent: {0})", this.ProcessorUtilizationInPercent);
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

            var otherObj = obj as ProcessorUtilizationInformation;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString());
            }

            return false;
        }
    }
}