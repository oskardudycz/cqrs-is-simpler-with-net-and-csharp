using Warehouse.Core;

namespace Warehouse.Products;

internal class GetProductsHandler: IQueryHandler<GetProducts, IReadOnlyList<ProductListItem>>
{
    private readonly IProductRepository repository;

    public GetProductsHandler(IProductRepository repository) =>
        this.repository = repository;

    public ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct) =>
        repository.GetProducts(query.Filter, query.Page, query.PageSize, ct);
}
