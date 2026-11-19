using Mediat.R.Interfaces;
using Mediat.R.Tests.Handlers;
using Mediat.R.Tests.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Mediat.R.Tests
{
    public class MediatorTests
    {
        [Fact()]
        public async Task Mediator_SendsCommandToHandler()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();
            services.AddTransient<ICommandHandler<TestCommand, SimpleResponse>>(
                service => new TestCommandHandler(cmd => $"Handled: {cmd.Input}"));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var result = await mediator.SendCommandAsync<TestCommand, SimpleResponse>(new TestCommand("Test"));

            Assert.Equal("Handled: Test", result.Result);
        }

        [Fact()]
        public async Task Mediator_SendsQueryToHandler()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();
            services.AddTransient<IQueryHandler<TestQuery, SimpleResponse>>(
                service => new TestQueryHandler(query => $"Result: {query.Input}"));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            var result = await mediator.SendQueryAsync<TestQuery, SimpleResponse>(new TestQuery("Test"));

            Assert.Equal("Result: Test", result.Result);
        }

        [Fact()]
        public async Task Mediator_ThrowsWhenHandlerNotFound()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IMediator, Mediator>();
            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => mediator.SendCommandAsync<TestCommand, SimpleResponse>(new TestCommand("Test")));
        }
    }
}
