using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Warehouse.Core.Commands;
using static Microsoft.AspNetCore.Http.Results;

namespace Warehouse.Products.RegisteringProduct;

public record RegisterProductRequest(
    string SKU,
    string Name,
    string? Description
);

internal static class Route
{
    internal static IEndpointRouteBuilder UseRegisterProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/products/", Handle);

        return endpoints;
    }

    private static async Task<IResult> Handle(
        ICommandHandler<RegisterProduct> commandHandler,
        RegisterProductRequest request,
        CancellationToken ct)
    {
        var (sku, name, description) = request;
        var productId = Guid.NewGuid();

        var command = RegisterProduct.From(productId, sku, name, description);

        await commandHandler.Handle(command, ct);

        return Created($"/api/products/{productId}", productId);
    }
}
