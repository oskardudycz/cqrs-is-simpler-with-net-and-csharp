using Warehouse.Core;

namespace Warehouse.Products;

internal class RegisterProductHandler: ICommandHandler<RegisterProduct>
{
    private readonly IProductRepository repository;

    public RegisterProductHandler(IProductRepository repository) =>
        this.repository = repository;

    public async ValueTask Handle(RegisterProduct command, CancellationToken ct)
    {
        if (await repository.ProductWithSKUExists(command.SKU, ct))
            throw new InvalidOperationException(
                $"Product with SKU `{command.SKU} already exists.");

        var product = new Product(
            command.ProductId,
            command.SKU,
            command.Name,
            command.Description
        );

        await repository.Add(product, ct);
    }
}
