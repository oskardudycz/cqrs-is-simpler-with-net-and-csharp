using System.Diagnostics.CodeAnalysis;
using Warehouse.Products.Primitives;

namespace Warehouse.Products;

internal record Product
{
    public required ProductId Id { get; init; }

    /// <summary>
    /// The Stock Keeping Unit (SKU), i.e. a merchant-specific identifier for a product or service, or the product to which the offer refers.
    /// </summary>
    /// <returns></returns>
    public required SKU Sku { get; init; }

    /// <summary>
    /// Product Name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Optional Product description
    /// </summary>
    public string? Description { get; init; }

    // Note: this is needed because we're using SKU DTO.
    // It would work if we had just primitives
    // Should be fixed in .NET 6
    private Product() { }

    [SetsRequiredMembers]
    public Product(ProductId id, SKU sku, string name, string? description)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
    }
}
