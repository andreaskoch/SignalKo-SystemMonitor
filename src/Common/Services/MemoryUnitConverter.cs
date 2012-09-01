namespace SignalKo.SystemMonitor.Common.Services
{
    public class MemoryUnitConverter : IMemoryUnitConverter
    {
        private const float KiloBytesPerMegabyte = 1024f;

        public double ConvertMegabyteToGigabyte(float megabytes)
        {
            return megabytes / KiloBytesPerMegabyte;
        }

        public double ConvertBytesToGigabyte(float bytes)
        {
            var megabytes = this.ConvertBytesToMegabytes(bytes);
            return megabytes / KiloBytesPerMegabyte;
        }

        public double ConvertBytesToMegabytes(float bytes)
        {
            return (bytes / KiloBytesPerMegabyte) / KiloBytesPerMegabyte;
        }
    }
}