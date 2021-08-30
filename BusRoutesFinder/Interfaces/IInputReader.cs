namespace BusRoutesFinder.Interfaces
{
    public interface IInputReader
    {
        string ReadBusRoutesFilePath();
        string ReadDepartureTime();
        string ReadEndBusStop();
        string ReadStartBusStop();
    }
}