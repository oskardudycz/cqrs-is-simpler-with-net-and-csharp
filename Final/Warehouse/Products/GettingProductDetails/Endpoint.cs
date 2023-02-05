using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using static Microsoft.AspNetCore.Http.Results;

namespace Warehouse.Products.GettingProductDetails;

internal static class GetProductDetailsEndpoint
{
    internal static IEndpointRouteBuilder UseGetProductDetailsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/api/products/{id:guid}",
                async (
                    IQueryable<Product> queryable,
                    Guid id,
                    CancellationToken ct
                ) =>
                {
                    var query = GetProductDetails.Create(id);

                    var result = await queryable.Query(query, ct);

                    return result != null ? Ok(result) : NotFound();
                })
            .Produces<Product?>()
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}
