using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Requests;
using Warehouse.Products;

namespace Warehouse.Controllers;

[Route("api/products")]
public class ProductsCommandsController: Controller
{
    private readonly ProductsApplicationService productsApplicationService;

    public ProductsCommandsController(ProductsApplicationService productsApplicationService) =>
        this.productsApplicationService = productsApplicationService;

    [HttpPost]
    public async Task<IActionResult> Register(RegisterProductRequest request, CancellationToken ct)
    {
        var (sku, name, description) = request;
        var productId = Guid.NewGuid();

        var command = RegisterProduct.Create(productId, sku, name, description);

        await productsApplicationService.Handle(command, ct);

        return Created($"api/products/{productId}", productId);
    }
}
