﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Core;
using Warehouse.Storage;

namespace Warehouse.Products;

internal static class Configuration
{
    public static IServiceCollection AddProductServices(this IServiceCollection services) =>
        services
            .AddScoped<ProductsCommandService>(s =>
            {
                var dbContext = s.GetRequiredService<WarehouseDBContext>();
                return new ProductsCommandService(dbContext.AddAndSave, dbContext.ProductWithSKUExists);
            })
            .AddScoped<ProductsQueryService>(s =>
            {
                var dbContext = s.GetRequiredService<WarehouseDBContext>();
                return new ProductsQueryService(dbContext.Set<Product>().AsNoTracking());
            });

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
