using Microsoft.EntityFrameworkCore;
using Warehouse.Core.Primitives;
using Warehouse.Core.Queries;
using Warehouse.Products.Primitives;

namespace Warehouse.Products.GettingProductDetails;

internal class HandleGetProductDetails: IQueryHandler<GetProductDetails, ProductDetails?>
{
    private readonly IQueryable<Product> products;

    public HandleGetProductDetails(IQueryable<Product> products)
    {
        this.products = products;
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

public record GetProductDetails(ProductId ProductId)
{
    public static GetProductDetails Create(Guid productId) =>
        new(ProductId.From(productId));
}

public record ProductDetails(
    Guid Id,
    string Sku,
    string Name,
    string? Description
);
