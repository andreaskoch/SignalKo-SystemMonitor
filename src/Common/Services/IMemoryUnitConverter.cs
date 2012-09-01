namespace SignalKo.SystemMonitor.Common.Services
{
    public interface IMemoryUnitConverter
    {
        double ConvertMegabyteToGigabyte(float megabytes);

        double ConvertBytesToGigabyte(float bytes);

        double ConvertBytesToMegabytes(float bytes);
    }
}