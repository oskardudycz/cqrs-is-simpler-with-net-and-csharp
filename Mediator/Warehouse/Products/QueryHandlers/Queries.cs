using Warehouse.Core;

namespace Warehouse.Products;

public record GetProducts(string? Filter, int Page, int PageSize)
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public static GetProducts From(string? filter, int? page, int? pageSize) =>
        new(
            filter,
            (page ?? DefaultPage).AssertPositive(),
            (pageSize ?? DefaultPageSize).AssertPositive()
        );
}

public record ProductListItem(
    Guid Id,
    string Sku,
    string Name
);

public record GetProductDetails(ProductId ProductId)
{
    public static GetProductDetails From(Guid productId) =>
        new(ProductId.From(productId));
}

public record ProductDetails(
    Guid Id,
    string Sku,
    string Name,
    string? Description
);
