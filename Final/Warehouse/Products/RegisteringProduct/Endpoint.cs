using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Warehouse.Core.Entities;
using Warehouse.Storage;
using static Microsoft.AspNetCore.Http.Results;
using static Warehouse.Products.RegisteringProduct.RegisterProductHandler;

namespace Warehouse.Products.RegisteringProduct;

public record RegisterProductRequest(
    string SKU,
    string Name,
    string? Description
);

internal static class RegisterProductEndpoint
{
    internal static IEndpointRouteBuilder UseRegisterProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "api/products/",
                async (
                    WarehouseDBContext dbContext,
                    RegisterProductRequest request,
                    CancellationToken ct
                ) =>
                {
                    var (sku, name, description) = request;
                    var productId = Guid.NewGuid();

                    var command = RegisterProduct.From(productId, sku, name, description);

                    await Handle(
                        dbContext.AddAndSave,
                        dbContext.ProductWithSKUExists,
                        command,
                        ct
                    );

                    return Created($"/api/products/{productId}", productId);
                })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return endpoints;
    }
}
