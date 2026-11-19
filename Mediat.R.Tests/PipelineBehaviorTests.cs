using Mediat.R.Behaviors;
using Mediat.R.Tests.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mediat.R.Tests
{
    public class PipelineBehaviorTests
    {
        [Fact()]
        public async Task LoggingBehavior_ShouldCallNextDelegate()
        {
            var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
            var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

            bool delegateCalled = false;
            Func<Task<TestResponse>> next = () =>
            {
                delegateCalled = true;
                return Task.FromResult(new TestResponse("Success"));
            };

            var result = await behavior.HandleAsync(new TestRequest("Test"), next, CancellationToken.None);

            Assert.True(delegateCalled);
            Assert.Equal("Success", result.Result);

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Completed: TestResponse")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact()]
        public async Task LoggingBehavior_ShouldPreserveExceptions()
        {
            var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
            var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

            var expectedException = new InvalidOperationException("Test exception");
            Func<Task<TestResponse>> next = () => Task.FromException<TestResponse>(expectedException);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await behavior.HandleAsync(new TestRequest("Test"), next, CancellationToken.None));

            Assert.Equal("Test exception", exception.Message);

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact()]
        public async Task LoggingBehavior_ShouldHandleCancellation()
        {
            var loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
            var behavior = new LoggingBehavior<TestRequest, TestResponse>(loggerMock.Object);

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Func<Task<TestResponse>> next = () =>
            {
                cts.Token.ThrowIfCancellationRequested();
                return Task.FromResult(new TestResponse("Success"));
            };

            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await behavior.HandleAsync(new TestRequest("Test"), next, cts.Token));

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestRequest")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
