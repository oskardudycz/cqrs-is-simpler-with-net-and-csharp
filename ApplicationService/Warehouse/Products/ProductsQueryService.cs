using Microsoft.EntityFrameworkCore;

namespace Warehouse.Products;

public class ProductsQueryService
{
    private readonly IQueryable<Product> products;

    public ProductsQueryService(IQueryable<Product> products)
    {
        this.products = products;
    }

    public async ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct)
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

        var result = await filteredProducts
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync(ct);

        var sth = result
            .Select(p => new ProductListItem(p.Id.Value, p.Sku.Value, p.Name))
            .ToList();

        return sth;
    }

    public async ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct)
    {
        var product = await products
            .SingleOrDefaultAsync(p => p.Id == query.ProductId, ct);

        if (product == null)
            return null;

        return new ProductDetails(
            product.Id.Value,
            product.Sku.Value,
            product.Name,
            product.Description
        );
    }
}
