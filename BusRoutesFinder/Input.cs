using BusRoutesFinder.Extensions;
using BusRoutesFinder.Interfaces;
using BusRoutesFinder.Models;
using System;
using System.IO;

namespace BusRoutesFinder
{
    public class Input
    {
        private static readonly BusRoutes _empty = new BusRoutes(0); 
        private readonly IInputReader _inputReader;
        private readonly IMessageWriter _messageWriter;
        private readonly IStreamReaderProvider _streamReaderProvider;
        private int _numberOfStops;

        public Input(IInputReader inputReader, IMessageWriter messageWriter, IStreamReaderProvider streamReaderProvider)
        {
            _inputReader = inputReader;
            _messageWriter = messageWriter;
            _streamReaderProvider = streamReaderProvider;
        }

        public InputData Parse()
        {
            var inputData = new InputData 
            {
                BusRoutes = ParseBusRoutesFile(),
                StartBusStop = ParseBusStopNumber(_inputReader.ReadStartBusStop),
                EndBusStop = ParseBusStopNumber(_inputReader.ReadEndBusStop),
                DepartureTime = ParseDepartureTime(),
            };

            return inputData;
        }

        private BusRoutes ParseBusRoutesFile()
        {
            using var fileReader = _streamReaderProvider.GetStreamReader();
            var numberOfRoutes = fileReader.ReadSingleNumber() ?? 0;
            _numberOfStops = fileReader.ReadSingleNumber() ?? 0;

            if (numberOfRoutes == 0 || _numberOfStops == 0)
            {
                return _empty;
            }

            var departureTimes = fileReader.ReadDepartureTimes();
            var routeCosts = fileReader.ReadRouteCosts();

            var routes = new BusRoutes(numberOfRoutes);

            for (int i = 0; i < numberOfRoutes; i++)
            {
                var routeSegmentsData = fileReader.ReadRouteData();
                int numberOfStopsInRoute = routeSegmentsData[0];
                routeSegmentsData = routeSegmentsData[1..];

                var route = new BusRoute
                {
                    Cost = routeCosts[i],
                    DepartureTime = departureTimes[i],
                    Segments = new RouteSegments(numberOfStopsInRoute),
                };

                routes.Add(route);

                for (int j = 0; j < numberOfStopsInRoute; j++)
                {
                    route.Segments.Add(new RouteSegment
                    {
                        StartBusStop = routeSegmentsData[j],
                        DurationInMunutes = routeSegmentsData[j + numberOfStopsInRoute],
                    });
                }
            }

            return routes;
        }

        private int ParseBusStopNumber(Func<string> readInput)
        {
            int busStop;

            while (!int.TryParse(readInput?.Invoke(), out busStop) || !IsValidStopNumber(busStop))
            {
                _messageWriter.Write("Entered invalid bus stop number. Please re-enter.");
            }

            return busStop;
        }
        
        private DateTime ParseDepartureTime()
        {
            DateTime departureTime;

            while (!DateTime.TryParse(_inputReader.ReadDepartureTime() , out departureTime))
            {
                _messageWriter.Write("Entered invalid departure time. Please re-enter.");
            }

            return departureTime;
        }

        private bool IsValidStopNumber(int number)
        {
            return number >= 1 && number <= _numberOfStops;
        }
    }
}
