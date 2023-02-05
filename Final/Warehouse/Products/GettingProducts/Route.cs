using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Warehouse.Core.Queries;
using Warehouse.Products.GettingProductDetails;
using static Microsoft.AspNetCore.Http.Results;

namespace Warehouse.Products.GettingProducts;

public static class Route
{
    internal static IEndpointRouteBuilder UseGetProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/products", Handle);
        return endpoints;
    }

    private static async Task<IResult> Handle(
        IQueryHandler<GetProducts, IReadOnlyList<ProductListItem>> queryHandler,
        [FromQuery] string? filter,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken ct
    )
    {
        var query = GetProducts.From(filter, page, pageSize);

        var result = await queryHandler.Handle(query, ct);

        return Ok(result);
    }
}
