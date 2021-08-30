using System.IO;

namespace BusRoutesFinder.Interfaces
{
    public interface IStreamReaderProvider
    {
        StreamReader GetStreamReader();
    }
}
