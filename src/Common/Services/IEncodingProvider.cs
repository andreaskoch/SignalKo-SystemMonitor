using System.Text;

namespace SignalKo.SystemMonitor.Common.Services
{
    public interface IEncodingProvider
    {
        Encoding GetEncoding();
    }
}