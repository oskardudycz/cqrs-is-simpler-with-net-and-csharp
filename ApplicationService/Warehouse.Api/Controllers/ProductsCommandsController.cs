using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Requests;
using Warehouse.Products;

namespace Warehouse.Api.Controllers;

[Route("api/products")]
public class ProductsCommandsController: Controller
{
    private readonly ProductsCommandService productsCommandService;

    public ProductsCommandsController(ProductsCommandService productsCommandService) =>
        this.productsCommandService = productsCommandService;

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterProductRequest request, CancellationToken ct)
    {
        var (sku, name, description) = request;
        var productId = Guid.NewGuid();

        var command = RegisterProduct.From(productId, sku, name, description);

        await productsCommandService.Handle(command, ct);

        return Created($"api/products/{productId}", productId);
    }
}
