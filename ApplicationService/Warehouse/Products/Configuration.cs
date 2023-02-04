using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core.Entities;
using Warehouse.Storage;

namespace Warehouse.Products;

internal static class Configuration
{
    public static IServiceCollection AddProductServices(this IServiceCollection services) =>
        services
            .AddScoped<ProductsApplicationService>(s =>
            {
                var dbContext = s.GetRequiredService<WarehouseDBContext>();
                return new ProductsApplicationService(dbContext.AddAndSave, dbContext.ProductWithSKUExists);
            })
            .AddScoped<ProductsQueryService>(s =>
            {
                var dbContext = s.GetRequiredService<WarehouseDBContext>();
                return new ProductsQueryService(dbContext.Set<Product>().AsNoTracking());
            });

    public static void SetupProductsModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Sku);
    }
}
