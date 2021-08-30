using BusRoutesFinder.Models;
using System;
using Xunit;

namespace BusRoutesFinder.Test
{
    public class RouteCalculatorTests
    {
        [Fact]
        public void FindCheapestRoute_ZeroRoutesInInputData_ReturnsNegativeOne()
        {
            var inputData = new InputData
            {
                BusRoutes = new BusRoutes(0),
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 1,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindCheapestRouteIndex();

            Assert.Equal(-1, index);
        }

        [Fact]
        public void FindCheapestRoute_TheOnlyRouteInInputData_ReturnsZero()
        {
            var busRoutes = new BusRoutes(1);
            var routeSegments = new RouteSegments(1);
            routeSegments.Add(new RouteSegment
            {
                DurationInMunutes = 0,
                StartBusStop = 1
            });
            busRoutes.Add(new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = routeSegments,
            });
            var inputData = new InputData
            {
                BusRoutes = busRoutes,
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 1,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindCheapestRouteIndex();

            Assert.Equal(0, index);
        }

        [Fact]
        public void FindCheapestRoute_TwoRoutesInInputData_ReturnsIndexOfCheapest()
        {
            var busRoutes = new BusRoutes(2);
            var routeSegments = new RouteSegments(1);
            routeSegments.Add(new RouteSegment
            {
                DurationInMunutes = 0,
                StartBusStop = 1
            });
            var cheapestRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = routeSegments,
                Cost = 10,
            };
            var expensiveRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = routeSegments,
                Cost = 100,
            };
            busRoutes.Add(cheapestRoute);
            busRoutes.Add(expensiveRoute);
            var inputData = new InputData
            {
                BusRoutes = busRoutes,
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 1,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindCheapestRouteIndex();

            Assert.Equal(0, index);
        }

        [Fact]
        public void FindFastestRoute_ZeroRoutesInInputData_ReturnsNegativeOne()
        {
            var inputData = new InputData
            {
                BusRoutes = new BusRoutes(0),
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 1,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindFastestRouteIndex();

            Assert.Equal(-1, index);
        }

        [Fact]
        public void FindFastestRoute_TheOnlyRouteInInputData_ReturnsZero()
        {
            var busRoutes = new BusRoutes(1);
            var routeSegments = new RouteSegments(1);
            routeSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 1
            });
            routeSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 2
            });
            busRoutes.Add(new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = routeSegments,
            });
            var inputData = new InputData
            {
                BusRoutes = busRoutes,
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 2,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindFastestRouteIndex();

            Assert.Equal(0, index);
        }

        [Fact]
        public void FindFastestRoute_TwoRoutesInInputData_ReturnsIndexOfFastestRoute()
        {
            var busRoutes = new BusRoutes(2);
            var fastestRouteSegments = new RouteSegments(2);
            fastestRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 1
            });
            fastestRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 2
            });
            var slowRouteSegments = new RouteSegments(2);
            slowRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 100,
                StartBusStop = 1
            });
            slowRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 100,
                StartBusStop = 2
            });
            var fastestRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = fastestRouteSegments,
                Cost = 10,
            };
            var cheapRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = slowRouteSegments,
                Cost = 10,
            };
            busRoutes.Add(fastestRoute);
            busRoutes.Add(cheapRoute);
            var inputData = new InputData
            {
                BusRoutes = busRoutes,
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 2,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindFastestRouteIndex();

            Assert.Equal(0, index);
        }

        [Fact]
        public void FindFastestRoute_TwoRoutesInInputDataTheFirstStartsLaterThanOther_ReturnsIndexOfSecondRoute()
        {
            var busRoutes = new BusRoutes(2);
            var fastestRouteSegments = new RouteSegments(2);
            fastestRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 1
            });
            fastestRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 2
            });
            var slowRouteSegments = new RouteSegments(2);
            slowRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 1
            });
            slowRouteSegments.Add(new RouteSegment
            {
                DurationInMunutes = 10,
                StartBusStop = 2
            });
            var cheapRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:05"),
                Segments = slowRouteSegments,
                Cost = 10,
            };
            var fastestRoute = new BusRoute
            {
                DepartureTime = DateTime.Parse("13:00"),
                Segments = fastestRouteSegments,
                Cost = 10,
            };
            busRoutes.Add(cheapRoute);
            busRoutes.Add(fastestRoute);
            var inputData = new InputData
            {
                BusRoutes = busRoutes,
                DepartureTime = DateTime.Parse("13:00"),
                StartBusStop = 1,
                EndBusStop = 2,
            };
            var calculator = new RouteCalculator(inputData);

            var index = calculator.FindFastestRouteIndex();

            Assert.Equal(1, index);
        }
    }
}
