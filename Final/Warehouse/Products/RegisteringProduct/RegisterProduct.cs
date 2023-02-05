using Warehouse.Products.Primitives;

namespace Warehouse.Products.RegisteringProduct;

internal static class RegisterProductHandler
{
    internal static async Task Handle(
        Func<Product, CancellationToken, ValueTask> addProduct,
        Func<SKU, CancellationToken, ValueTask<bool>> productWithSKUExists,
        RegisterProduct command,
        CancellationToken ct
    )
    {
        var product = new Product(
            command.ProductId,
            command.SKU,
            command.Name,
            command.Description
        );

        if (await productWithSKUExists(command.SKU, ct))
            throw new InvalidOperationException(
                $"Product with SKU `{command.SKU} already exists.");

        await addProduct(product, ct);
    }
}

public record RegisterProduct(
    ProductId ProductId,
    SKU SKU,
    string Name,
    string? Description
)
{
    public static RegisterProduct From(Guid? id, string sku, string name, string? description) =>
        new(
            ProductId.From(id),
            SKU.From(sku),
            name.AssertNotEmpty(),
            description.AssertNullOrNotEmpty()
        );
}
