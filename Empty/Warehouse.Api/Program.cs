// CQRS

// Command

Console.WriteLine("sth");

public readonly record struct ProductId(Guid Value)
{
    public static ProductId From(Guid? productId) =>
        new(productId.AssertNotEmpty());
}

public readonly record struct SKU(string Value)
{
    public static SKU From(string? sku) =>
        new(sku.AssertMatchesRegex("[A-Z]{2,4}[0-9]{4,18}"));
}

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

// Query
public record GetProducts(string? Filter, int Page, int PageSize)
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public static GetProducts From(string? filter, int? page, int? pageSize) =>
        new(
            filter,
            page.GetValueOrDefault(DefaultPage).AssertPositive(),
            pageSize.GetValueOrDefault(DefaultPageSize).AssertPositive()
        );
}

public record ProductListItem(
    Guid Id,
    string Sku,
    string Name
);

public record GetProductDetails(ProductId ProductId)
{
    public static GetProductDetails Create(Guid productId) =>
        new(ProductId.From(productId));
}

public record ProductDetails(
    Guid Id,
    string Sku,
    string Name,
    string? Description
);

// Responsibility Segregation

interface ICommandHandler<in TCommand>
{
    ValueTask Handle(TCommand command, CancellationToken ct);
}

public class RegisterProductHandler: ICommandHandler<RegisterProduct>
{
    public ValueTask Handle(RegisterProduct command, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public class DeactivateProductHandler: ICommandHandler<DeactivateProduct>
{
    public ValueTask Handle(DeactivateProduct command, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

interface IQueryHandler<in TQuery, TResult>
{
    ValueTask<TResult> Handle(TQuery query, CancellationToken ct);
}

public class GetProductDetailsHandler: IQueryHandler<GetProductDetails, ProductDetails?>
{
    public ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public class GetProductsHandler: IQueryHandler<GetProducts, IReadOnlyList<ProductListItem>>
{
    public ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public class ProductsApplicationService
{
    public Task Handle(RegisterProduct command, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }

    public Task Handle(DeactivateProduct command, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public class ProductsQueriesService
{
    public Task Handle(RegisterProduct command, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public class ProductsQueryService
{
    public ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }

    public ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct)
    {
        throw new NotImplementedException("We'll get to that later");
    }
}

public partial class Program
{
}
