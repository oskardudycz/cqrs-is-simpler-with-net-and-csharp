﻿using Microsoft.EntityFrameworkCore;
using Warehouse.Storage;

namespace Warehouse.Products;

internal static class ProductsRepository
{
    public static ValueTask<bool> ProductWithSKUExists(this WarehouseDBContext dbContext, SKU productSKU,
        CancellationToken ct)
        => new(dbContext.Set<Product>().AnyAsync(product => product.Sku == productSKU, ct));
}
