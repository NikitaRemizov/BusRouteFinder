using BusRoutesFinder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using BusRoutesFinder.Models;

namespace BusRoutesFinder
{
    public class RouteCalculator
    {
        private const int NotFoundIndex = -1;

        private IEnumerable<BusRoute> MatchingRoutes
        {
            get
            {
                if (_matchingRoutes is null)
                {
                    _matchingRoutes = _inputData.BusRoutes
                        .Where(r => r.Segments.Exists(s => s.StartBusStop == _inputData.StartBusStop) && 
                               r.Segments.Exists(s => s.StartBusStop == _inputData.EndBusStop) &&
                               _inputData.DepartureTime.AddMinutes(_subroutesDuration[r]).Date == _inputData.DepartureTime.Date
                        )
                        .ToList();
                }
                return _matchingRoutes;
            }
        }

        private IEnumerable<BusRoute> _matchingRoutes;
        private readonly InputData _inputData;
        private readonly Dictionary<BusRoute, int> _subroutesDuration;

        public RouteCalculator(InputData inputData)
        {
            _inputData = inputData;
            _subroutesDuration = new Dictionary<BusRoute, int>(_inputData.BusRoutes.Count);
            foreach (var route in _inputData.BusRoutes)
            {
                _subroutesDuration.Add(route, EvaluateSubrouteDuration(route));
            }
        }

        public int FindCheapestRouteIndex()
        {
            var cheapestRoute = MatchingRoutes
                .OrderBy(r => r.Cost)
                .FirstOrDefault();

            if (cheapestRoute is null)
            {
                return NotFoundIndex;
            }

            return _inputData.BusRoutes.IndexOf(cheapestRoute);
        }

        public int FindFastestRouteIndex()
        {
            var fastestRoute = MatchingRoutes
                .OrderBy(r => _subroutesDuration[r])
                .FirstOrDefault();

            if (fastestRoute is null)
            {
                return NotFoundIndex;
            }

            return _inputData.BusRoutes.IndexOf(fastestRoute);
        }

        private int EvaluateSubrouteDuration(BusRoute route)
        {
            var segmentsOfSpecificRoute = route.Segments
                .EnumerateFrom(_inputData.StartBusStop)
                .TakeWhileInclusive(s => s.StartBusStop != _inputData.EndBusStop)
                .ToArray();

            if (segmentsOfSpecificRoute.Length == 0 || segmentsOfSpecificRoute[^1].StartBusStop != _inputData.EndBusStop)
            {
                return int.MaxValue;
            }

            var timeToWaitBeforeBusArrives = (CalculateNextTimeTheBusArrives(route) - _inputData.DepartureTime).TotalMinutes;
            return segmentsOfSpecificRoute.SkipLast(1).Sum(s => s.DurationInMunutes) + (int)timeToWaitBeforeBusArrives;
        }

        private DateTime CalculateNextTimeTheBusArrives(BusRoute route)
        {
            int roundTripTime = route.Segments.RoundTripTime;
            return route.DepartureTime.AddMinutes(
                ((int)Math.Ceiling((_inputData.DepartureTime - route.DepartureTime).TotalMinutes / roundTripTime)) * roundTripTime
            );
        }
    }
}
