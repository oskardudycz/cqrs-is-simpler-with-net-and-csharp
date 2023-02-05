namespace Warehouse.Products;

/// <summary>
///
/// </summary>
/// <param name="Id"></param>
/// <param name="Sku">The Stock Keeping Unit (SKU), i.e. a merchant-specific identifier for a product or service, or the product to which the offer refers.</param>
/// <param name="Name">Product Name</param>
/// <param name="Description">Optional Product description</param>
public record Product(
    ProductId Id,
    SKU Sku,
    string Name,
    string? Description)
{
}
