namespace SignalKo.SystemMonitor.Agent.Core.Sender.Configuration
{
    public interface IRESTServiceConfiguration
    {
        string BaseUrl { get; set; }

        string ResourcePath { get; set; }

        bool IsValid();
    }
}