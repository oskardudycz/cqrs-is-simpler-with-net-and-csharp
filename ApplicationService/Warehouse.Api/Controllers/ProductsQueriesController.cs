﻿using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Requests;
using Warehouse.Products;

namespace Warehouse.Api.Controllers;

[Route("api/products")]
public class ProductsQueriesController: Controller
{
    private readonly ProductsQueryService queryService;

    public ProductsQueriesController(ProductsQueryService queryService) =>
        this.queryService = queryService;

    [HttpGet]
    public ValueTask<IReadOnlyList<ProductListItem>> Get(
        [FromQuery] GetProductsRequest request,
        CancellationToken ct
    ) =>
        queryService.Handle(GetProducts.From(request.Filter, request.Page, request.PageSize), ct);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var product = await queryService.Handle(GetProductDetails.From(id), ct);

        return product != null ? Ok(product) : NotFound();
    }
}
