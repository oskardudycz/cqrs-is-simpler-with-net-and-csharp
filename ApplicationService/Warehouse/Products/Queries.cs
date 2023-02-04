using Warehouse.Core.Primitives;

namespace Warehouse.Products;

public record GetProducts
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    private GetProducts(string? filter, int page, int pageSize)
    {
        Filter = filter;
        Page = page;
        PageSize = pageSize;
    }

    public string? Filter { get; }

    public int Page { get; }

    public int PageSize { get; }

    public static GetProducts Create(string? filter, int? page, int? pageSize)
    {
        page ??= DefaultPage;
        pageSize ??= DefaultPageSize;

        if (page <= 0)
            throw new ArgumentOutOfRangeException(nameof(page));

        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        return new GetProducts(filter, page.Value, pageSize.Value);
    }

    public void Deconstruct(out string? filter, out int page, out int pageSize)
    {
        filter = Filter;
        page = Page;
        pageSize = PageSize;
    }
}

public record ProductListItem(
    Guid Id,
    string Sku,
    string Name
);

public record GetProductDetails
{
    private GetProductDetails(Guid productId)
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }

    public static GetProductDetails Create(Guid productId)
    {
        return new GetProductDetails(productId.AssertNotEmpty(nameof(productId)));
    }
}

public record ProductDetails(
    Guid Id,
    string Sku,
    string Name,
    string? Description
);




