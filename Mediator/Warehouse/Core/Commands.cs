using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Core;

public interface ICommandHandler<in T>
{
    ValueTask Handle(T command, CancellationToken token);
}

public interface ICommandBus
{
    ValueTask Send<TCommand>(TCommand command, CancellationToken ct);
}

public class InMemoryCommandBus: ICommandBus
{
    private readonly IServiceProvider serviceProvider;

    public InMemoryCommandBus(IServiceProvider serviceProvider) =>
        this.serviceProvider = serviceProvider;

    public ValueTask Send<TCommand>(TCommand command, CancellationToken ct) =>
        serviceProvider.GetRequiredService<ICommandHandler<TCommand>>().Handle(command, ct);
}

public static class CommandHandlerConfiguration
{
    public static IServiceCollection AddInMemoryCommandBus(this IServiceCollection services) =>
        services.AddScoped<ICommandBus, InMemoryCommandBus>();

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
