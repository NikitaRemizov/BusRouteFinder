using BusRoutesFinder.Interfaces;
using System;
using System.IO;

namespace BusRoutesFinder
{
    public class StreamReaderProvider : IStreamReaderProvider
    {
        private readonly IInputReader _inputReader;
        private readonly IMessageWriter _messageWriter;

        public StreamReaderProvider(IInputReader inputReader, IMessageWriter messageWriter)
        {
            _inputReader = inputReader;
            _messageWriter = messageWriter;
        }

        public StreamReader GetStreamReader()
        {
            FileStream file = null;
            StreamReader fileReader = null;

            while (true)
            {
                try
                {
                    var filePath = _inputReader.ReadBusRoutesFilePath();
                    file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    fileReader = new StreamReader(file);
                    return fileReader;
                }
                catch (Exception ex) when ( ex is ArgumentException ||
                                            ex is PathTooLongException ||
                                            ex is IOException ||
                                            ex is FileNotFoundException)
                {
                    fileReader?.Dispose();
                    file?.Dispose();
                    _messageWriter.Write("The incorrect file path. Please re-enter.");
                    continue;
                }
            }
        }
    }
}
