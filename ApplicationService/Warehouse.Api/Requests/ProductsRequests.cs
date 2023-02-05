namespace Warehouse.Api.Requests;

public record RegisterProductRequest(
    string? SKU,
    string? Name,
    string? Description
);

public record GetProductsRequest(
    string? Filter,
    int? Page,
    int? PageSize
);
