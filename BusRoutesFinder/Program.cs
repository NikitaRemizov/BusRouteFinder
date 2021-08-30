namespace BusRoutesFinder
{
    class Program
    {
        static void Main()
        {
            var commandLineIO = new CommandLineIO();
            try
            {
                var inputStreamReaderProvider = new StreamReaderProvider(commandLineIO, commandLineIO);
                var input = new Input(commandLineIO, commandLineIO, inputStreamReaderProvider);
                var calculator = new RouteCalculator(input.Parse());

                commandLineIO.Write($"The cheapest route index: {calculator.FindCheapestRouteIndex()}");
                commandLineIO.Write($"The fastest route index: {calculator.FindFastestRouteIndex()}");
            }
            catch (System.Exception)
            {
                commandLineIO.Write("Unexpected error. Please restart application.");
                return;
            }
        }
    }
}
