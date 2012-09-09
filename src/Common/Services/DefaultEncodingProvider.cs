using System.Text;

namespace SignalKo.SystemMonitor.Common.Services
{
    public class DefaultEncodingProvider : IEncodingProvider
    {
        public Encoding GetEncoding()
        {
            return Encoding.UTF8;
        }
    }
}