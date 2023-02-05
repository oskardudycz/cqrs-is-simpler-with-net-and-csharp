using Microsoft.EntityFrameworkCore;
using Warehouse.Products.Primitives;

namespace Warehouse.Products.GettingProductDetails;

public record GetProductDetails(ProductId ProductId)
{
    public static GetProductDetails Create(Guid productId) =>
        new(ProductId.From(productId));
}

public record ProductDetails(
    Guid Id,
    string Sku,
    string Name,
    string? Description
);

internal static class GetProductDetailsQuery
{
    internal static async ValueTask<ProductDetails?> Query(
        this IQueryable<Product> products,
        GetProductDetails query,
        CancellationToken ct
    )
    {
        var product = await products
            .SingleOrDefaultAsync(p => p.Id == query.ProductId, ct);

        if (product == null)
            return null;

        return new ProductDetails(
            product.Id.Value,
            product.Sku.Value,
            product.Name,
            product.Description
        );
    }
}
