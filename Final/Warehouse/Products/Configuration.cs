using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Products.GettingProductDetails;
using Warehouse.Products.Primitives;
using Warehouse.Products.RegisteringProduct;
using Warehouse.Storage;

namespace Warehouse.Products;

internal static class Configuration
{
    public static IServiceCollection AddProductServices(this IServiceCollection services)
        => services
            .AddTransient(sp =>
                sp.GetRequiredService<WarehouseDBContext>().Set<Product>().AsNoTracking()
            );


    public static IEndpointRouteBuilder UseProductsEndpoints(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .UseRegisterProductEndpoint()
            .UseGetProductsEndpoint()
            .UseGetProductDetailsEndpoint();

    public static void SetupProductsModel(this ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product
            .Property(e => e.Id)
            .HasConversion(
                typed => typed.Value,
                plain => new ProductId(plain)
            );

        product
            .OwnsOne(e => e.Sku);
    }
}
