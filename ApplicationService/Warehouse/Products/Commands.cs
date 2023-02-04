using Warehouse.Products.Primitives;

namespace Warehouse.Products;

public record RegisterProduct
{
    private RegisterProduct(Guid productId, SKU sku, string name, string? description)
    {
        ProductId = productId;
        SKU = sku;
        Name = name;
        Description = description;
    }

    public Guid ProductId { get; }

    public SKU SKU { get; }

    public string Name { get; }

    public string? Description { get; }

    public static RegisterProduct Create(Guid? id, string? sku, string? name, string? description)
    {
        if (!id.HasValue || id == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrEmpty(sku)) throw new ArgumentOutOfRangeException(nameof(sku));
        if (string.IsNullOrEmpty(name)) throw new ArgumentOutOfRangeException(nameof(name));
        if (description is "") throw new ArgumentOutOfRangeException(nameof(name));

        return new RegisterProduct(id.Value, SKU.Create(sku), name, description);
    }
}


