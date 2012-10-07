using System;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class RESTServiceConfiguration : IRESTServiceConfiguration
    {
        public string Hostaddress { get; set; }

        public string Hostname { get; set; }

        public string ResourcePath { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Hostaddress) && !string.IsNullOrWhiteSpace(this.Hostname) && !string.IsNullOrWhiteSpace(this.ResourcePath);
        }

        public override string ToString()
        {
            return string.Format(
                "RESTServiceConfiguration (Hostaddress: {0}, Hostname: {1}, ResourcePath: {2})", this.Hostaddress, this.Hostname, this.ResourcePath);
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

            var otherObj = obj as RESTServiceConfiguration;
            if (otherObj != null)
            {
                return this.ToString().Equals(otherObj.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}