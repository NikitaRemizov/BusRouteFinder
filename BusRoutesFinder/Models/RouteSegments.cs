using System.Collections.Generic;
using System.Linq;

namespace BusRoutesFinder.Models
{
    public class RouteSegments : List<RouteSegment>
    {
        public int RoundTripTime
        {
            get 
            {
                if (_roundTripTime is null)
                {
                    _roundTripTime = this.Sum(s => s.DurationInMunutes);
                }
                return (int) _roundTripTime; 
            }
            set 
            {
                _roundTripTime = value; 
            }
        }

        private int? _roundTripTime;

        public RouteSegments(int numberOfSegments)
            : base(numberOfSegments)
        {
        }

        public IEnumerable<RouteSegment> EnumerateFrom(int startBusStop)
        {
            if (startBusStop < 0)
            {
                yield break;
            }

            var currentIndex = FindIndex(segment => segment.StartBusStop == startBusStop);

            if (currentIndex < 0)
            {
                yield break;
            }

            for (int i = 0; i < Count; i++)
            {
                yield return this[currentIndex];
                currentIndex = (currentIndex + 1) % Count;
            }
        }
    }
}
