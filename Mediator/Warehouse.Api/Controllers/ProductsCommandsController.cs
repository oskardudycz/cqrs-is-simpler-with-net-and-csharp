using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Requests;
using Warehouse.Core;
using Warehouse.Products;

namespace Warehouse.Api.Controllers;

[Route("api/products")]
public class ProductsCommandsController: Controller
{
    private readonly ICommandBus commandBus;

    public ProductsCommandsController(ICommandBus commandBus) =>
        this.commandBus = commandBus;

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterProductRequest request, CancellationToken ct)
    {
        var (sku, name, description) = request;
        var productId = Guid.NewGuid();

        var command = RegisterProduct.From(productId, sku, name, description);

        await commandBus.Send(command, ct);

        return Created($"api/products/{productId}", productId);
    }
}
