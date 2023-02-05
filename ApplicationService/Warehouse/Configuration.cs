using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Products;
using Warehouse.Storage;

namespace Warehouse;

public static class WarehouseConfiguration
{
    public static IServiceCollection AddWarehouseServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<WarehouseDBContext>(
                options => options.UseNpgsql(
                    "PORT = 5432; HOST = 127.0.0.1; TIMEOUT = 15; POOLING = True; MINPOOLSIZE = 1; MAXPOOLSIZE = 100; COMMANDTIMEOUT = 20; DATABASE = 'postgres'; PASSWORD = 'Password12!'; USER ID = 'postgres'; searchpath = 'public'"))
            .AddProductServices();
    }

    public static IApplicationBuilder ConfigureWarehouse(this IApplicationBuilder app)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        if (environment == "Development")
        {
            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<WarehouseDBContext>();

            dbContext.Database.Migrate();
        }

        return app;
    }
}
