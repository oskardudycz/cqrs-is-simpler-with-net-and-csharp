using Microsoft.AspNetCore.Mvc;
using Warehouse.Products;

namespace Warehouse.Controllers;

[Route("api/[controller]")]
public class ProductsQueriesController: Controller
{
    private readonly ProductsQueryService queryService;

    public ProductsQueriesController(ProductsQueryService queryService) =>
        this.queryService = queryService;

    [HttpGet]
    public ValueTask<IReadOnlyList<ProductListItem>> Get(
        [FromQuery] string? filter,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken ct
    ) =>
        queryService.Handle(GetProducts.Create(filter, page, pageSize), ct);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var product = await queryService.Handle(GetProductDetails.Create(id), ct);

        return product != null ? Ok(product) : NotFound();
    }
}
