﻿namespace Warehouse.Products;

public class ProductsCommandService
{
    private readonly Func<Product, CancellationToken, ValueTask> addProduct;
    private readonly Func<SKU, CancellationToken, ValueTask<bool>> productWithSKUExists;

    internal ProductsCommandService(
        Func<Product, CancellationToken, ValueTask> addProduct,
        Func<SKU, CancellationToken, ValueTask<bool>> productWithSKUExists
    )
    {
        this.addProduct = addProduct;
        this.productWithSKUExists = productWithSKUExists;
    }

    public async Task Handle(RegisterProduct command, CancellationToken ct)
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
