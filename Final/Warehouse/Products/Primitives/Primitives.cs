namespace Warehouse.Products.Primitives;

public readonly record struct ProductId(Guid Value)
{
    public static ProductId From(Guid? productId) =>
        new(productId.AssertNotEmpty());
}

public record SKU(string Value)
{
    public static SKU From(string? sku) =>
        new(sku.AssertMatchesRegex("[A-Z]{2,4}[0-9]{4,18}"));
}
