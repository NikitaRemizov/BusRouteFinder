using System;
using System.IO;
using System.Linq;

namespace BusRoutesFinder.Extensions
{
    public static class StreamReaderExtensions
    {
        private static readonly DateTime[] _emptyDateTime = Array.Empty<DateTime>();
        private static readonly int[] _emptyInts = Array.Empty<int>();

        public static int? ReadSingleNumber(this StreamReader stream)
        {
            var numberAsString = stream.ReadLine();
            if (numberAsString is null)
            {
                return null;
            }

            if (!int.TryParse(numberAsString, out int number))
            {
                return null;
            }

            return number;
        }

        public static DateTime[] ReadDepartureTimes(this StreamReader stream)
        {
            var departureTimesString = stream.ReadLine();
            if (departureTimesString is null)
            {
                return _emptyDateTime;
            }

            return departureTimesString
                .Split(' ')
                .Select(s => DateTime.Parse(s))
                .ToArray();
        }

        public static int[] ReadRouteCosts(this StreamReader stream)
        {
            var routeCostsString = stream.ReadLine();
            if (routeCostsString is null)
            {
                return _emptyInts;
            }

            return routeCostsString
                .Split(' ')
                .Select(s => int.Parse(s))
                .ToArray();
        }

        public static int[] ReadRouteData(this StreamReader stream)
        {
            var routeSegmentsString = stream.ReadLine();

            if (routeSegmentsString is null)
            {
                return _emptyInts;
            }

            return routeSegmentsString
                .Split(' ')
                .Select(s => int.Parse(s))
                .ToArray();
        }
    }
}
