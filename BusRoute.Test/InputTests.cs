using System;
using Xunit;
using BusRoutesFinder;
using Moq;
using BusRoutesFinder.Interfaces;

namespace BusRoutesFinder.Test
{
    public class InputTests
    {
        public Mock<IMessageWriter> MessageWriterMock { get; }
        public Mock<IInputReader> InputReaderMock { get; }
        public Mock<IStreamReaderProvider> StreamReaderProviderMock { get; }
        public string TestFileString { get; set; } = TestData.BusRouteData;
        public Input Input 
            => new (InputReaderMock.Object, MessageWriterMock.Object, StreamReaderProviderMock.Object);

        public InputTests()
        {
            InputReaderMock = new Mock<IInputReader>(MockBehavior.Strict);
            MessageWriterMock = new Mock<IMessageWriter>(MockBehavior.Strict);
            StreamReaderProviderMock = new Mock<IStreamReaderProvider>(MockBehavior.Strict);

            InputReaderMock
                .Setup(ir => ir.ReadBusRoutesFilePath())
                .Returns(() => string.Empty);
            InputReaderMock
                .Setup(ir => ir.ReadStartBusStop())
                .Returns(() => TestData.StartBusStop.ToString());
            InputReaderMock
                .Setup(ir => ir.ReadEndBusStop())
                .Returns(() => TestData.EndBusStop.ToString());
            InputReaderMock
                .Setup(ir => ir.ReadDepartureTime())
                .Returns(() => TestData.DepartureTime.ToString());

            MessageWriterMock
                .Setup(mw => mw.Write(It.IsAny<string>()));

            StreamReaderProviderMock
                .Setup(s => s.GetStreamReader())
                .Returns(() => TestData.CreateStreamReader(TestFileString));
        }

        [Fact]
        public void Parse_ParseCalledOnceOnValidBusRouteData_TheDependantInterfaceMethodsCalledOnce()
        {
            Input.Parse();

            InputReaderMock
                .Verify(ir => ir.ReadStartBusStop(), Times.Once);
            InputReaderMock
                .Verify(ir => ir.ReadEndBusStop(), Times.Once);
            InputReaderMock
                .Verify(ir => ir.ReadDepartureTime(), Times.Once);
            StreamReaderProviderMock
                .Verify(s => s.GetStreamReader(), Times.Once);
        }

        [Fact]
        public void Parse_IInputReaderProvidesStartBusStop_InputDataContainsProvidedValue()
        {
            var inputData = Input.Parse();

            Assert.Equal(TestData.StartBusStop, inputData.StartBusStop);
        }

        [Fact]
        public void Parse_IInputReaderProvidesEndBusStop_InputDataContainsProvidedValue()
        {
            var inputData = Input.Parse();

            Assert.Equal(TestData.EndBusStop, inputData.EndBusStop);
        }

        [Fact]
        public void Parse_IInputReaderProvidesDepartureTime_InputDataContainsProvidedValue()
        {
            var inputData = Input.Parse();

            Assert.Equal(TestData.DepartureTime, inputData.DepartureTime);
        }

        [Theory]
        [InlineData("0\n2\n10:00\n2", 0)]
        [InlineData("1\n2\n12:00\n10\n2 1 2 1 1", 1)]
        [InlineData("2\n2\n12:00 12:00\n10 20\n2 1 3 1 1\n2 1 2 1 1", 2)]
        [InlineData("3\n2\n12:00 12:00 12:00\n10 20 20\n2 1 3 1 1\n2 1 2 1 1\n3 1 2 3 2 2 2", 3)]
        public void Parse_IInputReaderProvidesNBusRoutes_InputDataContainsNBusRoutes(string testFileString, int expectedNumberOfRoutes)
        {
            TestFileString = testFileString; 

            var inputData = Input.Parse();

            Assert.Equal(expectedNumberOfRoutes, inputData.BusRoutes.Count);
        }

        [Fact]
        public void Parse_IInputReaderProvidedNonExistentBusStopNumber_InputReaderReadStartBusStopCalledMoreThanOnce()
        {
            bool returnValidValue = true;
            InputReaderMock
                .Setup(ir => ir.ReadStartBusStop())
                .Returns(() =>
                {
                    returnValidValue = !returnValidValue;
                    return returnValidValue ? TestData.StartBusStop.ToString() : int.MaxValue.ToString();
                });

            Input.Parse();

            InputReaderMock
                .Verify(ir => ir.ReadStartBusStop(), Times.AtLeast(2));
        }

        [Fact]
        public void Parse_IInputReaderProvidedNonExistentBusStopNumber_InputReaderReadEndBusStopCalledMoreThanOnce()
        {
            bool returnValidValue = true;
            InputReaderMock
                .Setup(ir => ir.ReadEndBusStop())
                .Returns(() =>
                {
                    returnValidValue = !returnValidValue;
                    return returnValidValue ? TestData.EndBusStop.ToString() : int.MaxValue.ToString();
                });

            Input.Parse();

            InputReaderMock
                .Verify(ir => ir.ReadEndBusStop(), Times.AtLeast(2));
        }

        [Fact]
        public void Parse_IInputReaderProvidedNonExistentBusStopNumber_InputReaderReadDepartureTimeCalledMoreThanOnce()
        {
            bool returnValidValue = true;
            InputReaderMock
                .Setup(ir => ir.ReadDepartureTime())
                .Returns(() =>
                {
                    returnValidValue = !returnValidValue;
                    return returnValidValue ? TestData.DepartureTime.ToString() : string.Empty;
                });

            Input.Parse();

            InputReaderMock
                .Verify(ir => ir.ReadDepartureTime(), Times.AtLeast(2));
        }

        [Theory]
        [InlineData("1\n2\n12:00\n10\n2 1 2 1 1")]
        [InlineData("2\n2\n12:00 12:00\n10 20\n2 1 3 1 1\n2 1 2 1 1")]
        public void Parse_DifferentValidInputs_DoesNotThrowException(string testFileString)
        {
            TestFileString = testFileString;

            Input.Parse();
        }
    }
}
