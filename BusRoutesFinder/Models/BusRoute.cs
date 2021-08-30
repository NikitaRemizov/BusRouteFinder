using System;

namespace BusRoutesFinder.Models
{
    public class BusRoute
    {
        public DateTime DepartureTime { get; init; }
        public int Cost { get; init; }
        public RouteSegments Segments { get; init; }
    }
}
