using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Warehouse.Products.Primitives;
using static Microsoft.AspNetCore.Http.TypedResults;

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

internal static class GetProductDetailsConfig
{
    internal static IEndpointRouteBuilder UseGetProductDetailsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products/{id:guid}", Handle);
        return endpoints;
    }

    private static async Task<IResult> Handle(
        IQueryable<Product> queryable,
        Guid id,
        CancellationToken ct
    )
    {
        var query = GetProductDetails.Create(id);

        var result = await queryable.Query(query, ct);

        return result != null ? Ok(result) : NotFound();
    }

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
