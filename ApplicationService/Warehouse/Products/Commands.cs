using Warehouse.Core;

namespace Warehouse.Products;

public record RegisterProduct(
    ProductId ProductId,
    SKU SKU,
    string Name,
    string? Description
)
{
    public static RegisterProduct From(Guid? id, string? sku, string? name, string? description) =>
        new(
            ProductId.From(id),
            SKU.From(sku),
            name.AssertNotEmpty(),
            description.AssertNullOrNotEmpty()
        );
}

public record DeactivateProduct(
    ProductId ProductId
)
{
    public static DeactivateProduct From(Guid? id) =>
        new(ProductId.From(id));
}
