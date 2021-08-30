using BusRoutesFinder.Interfaces;
using System;

namespace BusRoutesFinder
{
    public class CommandLineIO : IInputReader, IMessageWriter
    {
        public string ReadBusRoutesFilePath()
        {
            Console.Write("Enter path to the bus routes file: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public string ReadStartBusStop()
        {
            Console.Write("Enter start bus stop number: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public string ReadEndBusStop()
        {
            Console.Write("Enter destination bus stop number: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public string ReadDepartureTime()
        {
            Console.Write("Enter departure time: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}