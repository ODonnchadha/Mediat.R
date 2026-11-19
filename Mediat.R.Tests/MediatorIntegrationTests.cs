using Mediat.R.Behaviors;
using Mediat.R.Interfaces;
using Mediat.R.Tests.Handlers;
using Mediat.R.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using static Mediat.R.Tests.Helpers.BehaviorHelpers;

namespace Mediat.R.Tests
{
    public class MediatorIntegrationTests
    {
        private readonly List<string> steps = new();

        [Fact()]
        public async Task Mediator_ExecutesBehaviorsInCorrectOrder()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(sp =>
                new TestDataCommandHandler(cmd =>
                {
                    steps.Add("Handler executed");
                    return new TestData($"Result: {cmd.Input}");
                }));

            services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
                new FirstTestBehavior<TestDataCommand, TestData>(steps));
            services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
                new SecondTestBehavior(steps));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();
            steps.Clear();

            var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(
                new TestDataCommand("test-pipeline"), CancellationToken.None);

            Assert.Equal("Result: test-pipeline", result.Value);
            Assert.Equal(steps, new List<string>
                {
                    "First behavior before",
                    "Second behavior before",
                    "Handler executed",
                    "Second behavior after",
                    "First behavior after"
                });
        }

        [Fact()]
        public async Task Mediator_ExecutesPipelineBehaviorsWithQueries()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<IQueryHandler<TestDataQuery, TestData>>(sp =>
                new TestDataQueryHandler(query =>
                {
                    steps.Add("Query handler executed");
                    return new TestData($"Query: {query.Input}");
                }));

           services.AddTransient<IPipelineBehavior<TestDataQuery, TestData>>(sp =>
               new FirstTestBehavior<TestDataQuery, TestData>(steps));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();
            steps.Clear();

           var result = await mediator.SendQueryAsync<TestDataQuery, TestData>(
               new TestDataQuery("test-query"), CancellationToken.None);

            Assert.Equal("Query: test-query", result.Value);
            Assert.Equal(steps, new List<string>
                {
                    "First behavior before",
                    "Query handler executed",
                    "First behavior after"
                });
        }

        [Fact()]
        public async Task Mediator_HandlesLoggingBehaviorCorrectly()
        {
            var services = new ServiceCollection();
            var loggerMock = new Mock<ILogger<LoggingBehavior<TestDataCommand, TestData>>>();
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(sp =>
                new TestDataCommandHandler(_ => new TestData("Test result")));
            services.AddTransient<IPipelineBehavior<TestDataCommand, TestData>>(sp =>
                new LoggingBehavior<TestDataCommand, TestData>(loggerMock.Object));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(
               new TestDataCommand("logging-test"), CancellationToken.None);

            Assert.Equal("Test result", result.Value);
            loggerMock.Verify(
               l => l.Log(
                   It.IsAny<LogLevel>(),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Starting: TestDataCommand")),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
               Times.Once);

            loggerMock.Verify(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Completed: TestData")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
