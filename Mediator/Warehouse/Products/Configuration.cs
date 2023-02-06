using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core;

namespace Warehouse.Products;

internal static class Configuration
{
    public static IServiceCollection AddProductServices(this IServiceCollection services) =>
        services
            .AddTransient<IProductRepository, ProductsRepository>()
            .AddCommandHandler<RegisterProduct, RegisterProductHandler>()
            .AddQueryHandler<GetProductDetails, ProductDetails?, GetProductDetailsHandler>()
            .AddQueryHandler<GetProducts, IReadOnlyList<ProductListItem>, GetProductsHandler>();

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
