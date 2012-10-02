using System;

namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public class RESTServiceConfiguration : IRESTServiceConfiguration
    {
        public string BaseUrl { get; set; }

        public string ResourcePath { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.BaseUrl) && !string.IsNullOrWhiteSpace(this.ResourcePath);
        }

        public override string ToString()
        {
            return string.Format("RESTServiceConfiguration (BaseUrl: {0}, ResourcePath: {1})", this.BaseUrl, this.ResourcePath);
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