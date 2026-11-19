using Mediat.R.Interfaces;
using Mediat.R.Tests.Models;

namespace Mediat.R.Tests.Helpers
{
    public static class BehaviorHelpers
    {
        public class FirstTestBehavior<TInput, TOutput> : IPipelineBehavior<TInput, TOutput>
            where TOutput : TestData
        {
            private readonly List<string> _steps;

            public FirstTestBehavior(List<string> steps)
            {
                _steps = steps;
            }

            public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
            {
                _steps.Add("First behavior before");
                var result = await next();
                _steps.Add("First behavior after");
                return result;
            }
        }

        public class SecondTestBehavior : IPipelineBehavior<TestDataCommand, TestData>
        {
            private readonly List<string> _steps;

            public SecondTestBehavior(List<string> steps)
            {
                _steps = steps;
            }

            public async Task<TestData> HandleAsync(TestDataCommand input, Func<Task<TestData>> next, CancellationToken cancellationToken)
            {
                _steps.Add("Second behavior before");
                var result = await next();
                _steps.Add("Second behavior after");
                return result;
            }
        }
    }
}
