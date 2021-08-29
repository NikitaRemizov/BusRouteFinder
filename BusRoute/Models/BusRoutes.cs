using System.Collections.Generic;

namespace BusRoutesFinder.Models
{
    public class BusRoutes : List<BusRoute>
    {
        public BusRoutes(int numberOfRoutes)
            : base(numberOfRoutes)
        {
        }
    }
}