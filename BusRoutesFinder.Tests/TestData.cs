using System;
using System.IO;
using System.Text;

namespace BusRoutesFinder.Test
{
    public static class TestData
    {
        public static string BusRouteData { get; } = "1\n2\n12:00\n10\n2 1 2 1 1";
        public static int StartBusStop { get; } = 1;
        public static int EndBusStop { get; } = 1;
        public static DateTime DepartureTime { get; } = DateTime.Parse("12:00");

        public static StreamReader CreateStreamReader(string source)
        {
            if (source is null)
            {
                return null;
            }
            byte[] byteArray = Encoding.ASCII.GetBytes(source);
            MemoryStream stream = new MemoryStream(byteArray);
            return new StreamReader(stream);
        }
    }
}
