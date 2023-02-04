namespace Warehouse.Api.Requests;

public record RegisterProductRequest(
    string? SKU,
    string? Name,
    string? Description
);
