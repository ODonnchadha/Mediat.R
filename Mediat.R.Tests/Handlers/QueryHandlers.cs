using Mediat.R.Interfaces;
using Mediat.R.Tests.Models;

namespace Mediat.R.Tests.Handlers
{
    public class TestQueryHandler(Func<TestQuery, string> handler) : IQueryHandler<TestQuery, SimpleResponse>
    {
        public Task<SimpleResponse> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new SimpleResponse(handler(query)));
        }
    }

    public class CancellableQueryHandler : IQueryHandler<TestQuery, SimpleResponse>
    {
        public Task<SimpleResponse> HandleAsync(TestQuery query, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(new SimpleResponse($"Query handled: {query.Input}"));
        }
    }

    public class TestDataQueryHandler(Func<TestDataQuery, TestData> handler) : IQueryHandler<TestDataQuery, TestData>
    {
        public Task<TestData> HandleAsync(TestDataQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(query));
        }
    }
}
