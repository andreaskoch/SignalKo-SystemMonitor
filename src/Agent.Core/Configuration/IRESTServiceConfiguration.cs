namespace SignalKo.SystemMonitor.Agent.Core.Configuration
{
    public interface IRESTServiceConfiguration
    {
        string BaseUrl { get; set; }

        string ResourcePath { get; set; }

        bool IsValid();
    }
}