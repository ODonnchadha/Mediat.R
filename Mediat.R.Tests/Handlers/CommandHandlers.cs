using Mediat.R.Interfaces;
using Mediat.R.Tests.Models;

namespace Mediat.R.Tests.Handlers
{
    public class TestCommandHandler(Func<TestCommand, string> handler) : ICommandHandler<TestCommand, SimpleResponse>
    {
        public Task<SimpleResponse> HandleAsync(TestCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new SimpleResponse(handler(command)));
        }
    }

    public class CancellableCommandHandler : ICommandHandler<TestCommand, SimpleResponse>
    {
        public Task<SimpleResponse> HandleAsync(TestCommand command, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(new SimpleResponse($"Command handled: {command.Input}"));
        }
    }

    public class TestDataCommandHandler(Func<TestDataCommand, TestData> handler) : ICommandHandler<TestDataCommand, TestData>
    {
        public Task<TestData> HandleAsync(TestDataCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(command));
        }
    }
}
