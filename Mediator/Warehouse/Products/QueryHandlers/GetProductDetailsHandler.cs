using Warehouse.Core;

namespace Warehouse.Products;

internal class GetProductDetailsHandler: IQueryHandler<GetProductDetails, ProductDetails?>
{
    private readonly IProductRepository repository;

    public GetProductDetailsHandler(IProductRepository repository) =>
        this.repository = repository;

    public ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct) =>
        repository.GetProductDetails(query.ProductId, ct);
}
