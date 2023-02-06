using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static Microsoft.AspNetCore.Http.Results;

namespace Warehouse.Products.GettingProducts;

internal static class GetProductsEndpoint
{
    internal static IEndpointRouteBuilder UseGetProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/api/products",
                async (
                    IQueryable<Product> products,
                    [FromQuery] string? filter,
                    [FromQuery] int? page,
                    [FromQuery] int? pageSize,
                    CancellationToken ct
                ) =>
                {
                    var query = GetProducts.From(filter, page, pageSize);

                    var result = await products.Query(query, ct);

                    return Ok(result);
                })
            .Produces<IReadOnlyList<Product>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}
