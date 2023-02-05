using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Core.Commands;

public interface ICommandHandler<in T>
{
    Task Handle(T command, CancellationToken token);
}

public static class CommandHandlerConfiguration
{
    public static IServiceCollection AddCommandHandler<T, TCommandHandler>(
        this IServiceCollection services,
        Func<IServiceProvider, TCommandHandler>? configure = null
    ) where TCommandHandler : class, ICommandHandler<T>
    {
        if (configure == null)
        {
            services.AddTransient<TCommandHandler, TCommandHandler>();
            services.AddTransient<ICommandHandler<T>, TCommandHandler>();
        }
        else
        {
            services.AddTransient<TCommandHandler, TCommandHandler>(configure);
            services.AddTransient<ICommandHandler<T>, TCommandHandler>(configure);
        }

        return services;
    }
}
