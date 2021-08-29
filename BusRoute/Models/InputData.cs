using System;

namespace BusRoutesFinder.Models
{
    public class InputData
    {
        public BusRoutes BusRoutes { get; init; }
        public int StartBusStop { get; init; }
        public int EndBusStop { get; init; }
        public DateTime DepartureTime { get; init; }
    }
}