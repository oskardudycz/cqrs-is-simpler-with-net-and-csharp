using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Core;

public interface IQueryHandler<in TQuery, TResult>
{
    ValueTask<TResult> Handle(TQuery query, CancellationToken ct);
}

public interface IQueryBus
{
    ValueTask<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct);
}

public class InMemoryQueryBus: IQueryBus
{
    private readonly IServiceProvider serviceProvider;

    public InMemoryQueryBus(IServiceProvider serviceProvider) =>
        this.serviceProvider = serviceProvider;

    public ValueTask<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken ct) =>
        serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>().Handle(query, ct);
}

public static class QueryHandlerConfiguration
{
    public static IServiceCollection AddInMemoryQueryBus(this IServiceCollection services) =>
        services.AddScoped<IQueryBus, InMemoryQueryBus>();

    public static IServiceCollection AddQueryHandler<T, TResult, TQueryHandler>(
        this IServiceCollection services,
        Func<IServiceProvider, TQueryHandler>? configure = null
    ) where TQueryHandler : class, IQueryHandler<T, TResult>
    {
        if (configure == null)
        {
            services.AddTransient<TQueryHandler, TQueryHandler>();
            services.AddTransient<IQueryHandler<T, TResult>, TQueryHandler>();
        }
        else
        {
            services.AddTransient<TQueryHandler, TQueryHandler>(configure);
            services.AddTransient<IQueryHandler<T, TResult>, TQueryHandler>(configure);
        }

        return services;
    }
}
