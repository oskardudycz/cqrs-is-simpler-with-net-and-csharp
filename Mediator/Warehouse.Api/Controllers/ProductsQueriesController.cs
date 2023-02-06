using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Requests;
using Warehouse.Core;
using Warehouse.Products;

namespace Warehouse.Api.Controllers;

[Route("api/products")]
public class ProductsQueriesController: Controller
{
    private readonly IQueryBus queryBus;

    public ProductsQueriesController(IQueryBus queryBus) =>
        this.queryBus = queryBus;

    [HttpGet]
    public ValueTask<IReadOnlyList<ProductListItem>> Get(
        [FromQuery] GetProductsRequest request,
        CancellationToken ct
    ) =>
        queryBus.Query<GetProducts, IReadOnlyList<ProductListItem>>(
            GetProducts.From(request.Filter, request.Page, request.PageSize), ct
        );

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var product = await queryBus.Query<GetProductDetails, ProductDetails?>(GetProductDetails.From(id), ct);

        return product != null ? Ok(product) : NotFound();
    }
}
