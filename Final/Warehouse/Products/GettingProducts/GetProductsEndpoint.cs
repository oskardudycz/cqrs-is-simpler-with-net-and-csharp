using Microsoft.EntityFrameworkCore;

namespace Warehouse.Products.GettingProducts;

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

internal static class GetProductsQuery
{
    internal static async ValueTask<IReadOnlyList<ProductListItem>> Query(
        this IQueryable<Product> products,
        GetProducts query,
        CancellationToken ct
    )
    {
        var (filter, page, pageSize) = query;

        var filteredProducts = string.IsNullOrEmpty(filter)
            ? products
            : products
                .Where(p =>
                    p.Sku.Value.Contains(query.Filter!) ||
                    p.Name.Contains(query.Filter!) ||
                    p.Description!.Contains(query.Filter!)
                );

        return await filteredProducts
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Select(p => new ProductListItem(p.Id.Value, p.Sku.Value, p.Name))
            .ToListAsync(ct);
    }
}
