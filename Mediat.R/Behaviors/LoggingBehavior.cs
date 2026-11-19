using Mediat.R.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mediat.R.Behaviors;

public class LoggingBehavior<TInput, TOutput>(ILogger<LoggingBehavior<TInput, TOutput>> logger)
    : IPipelineBehavior<TInput, TOutput>
{
    public async Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting: {TypeName}", typeof(TInput).Name);

        var result = await next();

        logger.LogInformation("Completed: {TypeName}", typeof(TOutput).Name);

        return result;
    }
}
