using Mediat.R.Behaviors;
using Mediat.R.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Mediat.R.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
