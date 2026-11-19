using Mediat.R.Interfaces;
using Mediat.R.Tests.Handlers;
using Mediat.R.Tests.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Mediat.R.Tests
{
    public class CommandAndQueryTests
    {
        private readonly ServiceProvider provider;

        public CommandAndQueryTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IMediator, Mediator>();
            services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(
                service => new TestCommandHandler(cmd => $"Command executed: {cmd.Input}"));
            services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(
                service => new TestQueryHandler(query => $"Query executed: {query.Input}"));
            services.AddTransient<ICommandHandler<TestDataCommand, TestData>>(
                service => new TestDataCommandHandler(cmd => new TestData(cmd.Input)));
            services.AddTransient<IQueryHandler<TestDataQuery, TestData>>(
                service => new TestDataQueryHandler(query => new TestData(query.Input)));

            this.provider = services.BuildServiceProvider();
        }

        [Fact()]
        public async Task Mediator_ExecuteCommand_ShouldExecuteCommandHandler()
        {
            var mediator = provider.GetRequiredService<IMediator>();
            var command = new TestCommand("Test command");

            var result = await mediator.SendCommandAsync<TestCommand, SimpleResponse>(command);

            Assert.NotNull(result);
            Assert.Equal("Command executed: Test command", result.Result);
        }

        [Fact()]
        public async Task Mediator_ExecuteCommand_WithCancellationToken_ShouldUseToken()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(sp =>
                new CancellableCommandHandler());

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();
            var command = new TestCommand("Test command");

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await mediator.SendCommandAsync<TestCommand, SimpleResponse>(command, cts.Token));
        }

        [Fact()]
        public async Task Mediator_ExecuteQuery_ShouldExecuteQueryHandler()
        {
            var mediator = provider.GetRequiredService<IMediator>();
            var query = new TestQuery("Test query");

            var result = await mediator.SendQueryAsync<TestQuery, SimpleResponse>(query);

            Assert.NotNull(result);
            Assert.Equal("Query executed: Test query", result.Result);
        }

        [Fact()]
        public async Task Mediator_ExecuteQuery_WithCancellationToken_ShouldUseToken()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();

            services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(
                service => new CancellableQueryHandler());

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();
            var query = new TestQuery("Test query");

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await mediator.SendQueryAsync<TestQuery, SimpleResponse>(query, cts.Token));
        }

        [Fact()]
        public async Task Mediator_ExecuteCommand_WithData_ShouldReturnData()
        {
            var mediator = provider.GetRequiredService<IMediator>();
            var command = new TestDataCommand("Test data");

            var result = await mediator.SendCommandAsync<TestDataCommand, TestData>(command);

            Assert.NotNull(result);
            Assert.Equal("Test data", result.Value);
        }

        [Fact()]
        public async Task Mediator_ExecuteQuery_WithData_ShouldReturnData()
        {
            var mediator = provider.GetRequiredService<IMediator>();
            var query = new TestDataQuery("Test data");

            var result = await mediator.SendQueryAsync<TestDataQuery, TestData>(query);

            Assert.NotNull(result);
            Assert.Equal("Test data", result.Value);
        }
    }
}
