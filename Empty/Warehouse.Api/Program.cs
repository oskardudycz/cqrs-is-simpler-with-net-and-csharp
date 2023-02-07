// ReSharper disable ArrangeTypeModifiers
// ReSharper disable ClassNeverInstantiated.Global

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Core;

Console.WriteLine("🇸🇪 👋 Hey Swetugg! 🇸🇪 ");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<WarehouseDBContext>(
        options => options.UseNpgsql("name=ConnectionStrings:WarehouseDB")
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();

    // Kids, do not try this at home!
    using var scope = app.Services.CreateScope();
    await using var db = scope.ServiceProvider.GetRequiredService<WarehouseDBContext>();
    await db.Database.MigrateAsync();
}

// Register new product
app.MapPost(
    "api/products/",
    async Task<Results<Created, BadRequest, Conflict>>(
        WarehouseDBContext db,
        RegisterProductRequest request,
        CancellationToken ct
    ) =>
    {
        var productId = Guid.NewGuid();
        var (sku, name, description) = request;

        var product = new Product(
            ProductId.From(productId),
            SKU.From(sku),
            name.AssertNotEmpty(),
            description.AssertNullOrNotEmpty()
        );

        if (await db.Set<Product>().AnyAsync(p => p.Sku.Value == sku, ct))
            return TypedResults.Conflict();

        db.Add(product);
        await db.SaveChangesAsync(ct);

        return TypedResults.Created($"/api/products/{productId}");
    }
);

// Get Product Details by Id
app.MapGet(
    "/api/products/{id:guid}",
    async Task<Results<Ok<ProductDetails>, NotFound>>(
            WarehouseDBContext db,
            Guid id,
            CancellationToken ct
        ) =>
        await db.FindAsync<Product>(new[] { ProductId.From(id) })
            is { } product
            ? TypedResults.Ok(
                new ProductDetails(
                    product.Id.Value,
                    product.Sku.Value,
                    product.Name,
                    product.Description
                )
            )
            : TypedResults.NotFound()
);

// Get Products
app.MapGet(
    "/api/products",
    async Task<Results<Ok<List<ProductListItem>>, BadRequest, NotFound>>(
        WarehouseDBContext db,
        string? filter,
        int? page,
        int? pageSize,
        CancellationToken ct
    ) =>
    {
        var products = db.Products.AsNoTracking();

        var filteredProducts = string.IsNullOrEmpty(filter)
            ? products
            : products
                .Where(p =>
                    p.Sku.Value.Contains(filter) ||
                    p.Name.Contains(filter) ||
                    p.Description!.Contains(filter)
                );

        page ??= 1;
        pageSize ??= 10;

        return TypedResults.Ok(
            await filteredProducts
                .Skip(pageSize.Value * (page.Value - 1))
                .Take(pageSize.Value)
                .Select(p => new ProductListItem(p.Id.Value, p.Sku.Value, p.Name))
                .ToListAsync(ct)
        );
    }
);

app.Run();

public record RegisterProductRequest(
    string SKU,
    string Name,
    string? Description
);

// CQRS

// Command
readonly record struct ProductId(Guid Value)
{
    public static ProductId From(Guid? productId) =>
        new(productId.AssertNotEmpty());
}

record SKU(string Value)
{
    public static SKU From(string? sku) =>
        new(sku.AssertMatchesRegex("[A-Z]{2,4}[0-9]{4,18}"));
}

record RegisterProduct(
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

record DeactivateProduct(
    ProductId ProductId
)
{
    public static DeactivateProduct From(Guid? id) =>
        new(ProductId.From(id));
}

// Query
record GetProducts(string? Filter, int Page, int PageSize)
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

record GetProductDetails(ProductId ProductId)
{
    static GetProductDetails From(Guid productId) =>
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

class RegisterProductHandler: ICommandHandler<RegisterProduct>
{
    public ValueTask Handle(RegisterProduct command, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

class DeactivateProductHandler: ICommandHandler<DeactivateProduct>
{
    public ValueTask Handle(DeactivateProduct command, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

interface IQueryHandler<in TQuery, TResult>
{
    ValueTask<TResult> Handle(TQuery query, CancellationToken ct);
}

class GetProductDetailsHandler: IQueryHandler<GetProductDetails, ProductDetails?>
{
    public ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

class GetProductsHandler: IQueryHandler<GetProducts, IReadOnlyList<ProductListItem>>
{
    public ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

class ProductsApplicationService
{
    public Task Handle(RegisterProduct command, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");

    public Task Handle(DeactivateProduct command, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

class ProductsQueryService
{
    ValueTask<ProductDetails?> Handle(GetProductDetails query, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");

    ValueTask<IReadOnlyList<ProductListItem>> Handle(GetProducts query, CancellationToken ct) =>
        throw new NotImplementedException("We'll get to that later");
}

record Product
{
    public required ProductId Id { get; init; }

    public required SKU Sku { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }

    // Note: this is needed because we're using SKU DTO.
    // It would work if we had just primitives or strongly-typed keys
    private Product() { }

    [SetsRequiredMembers]
    public Product(ProductId id, SKU sku, string name, string? description)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Description = description;
    }
}

class WarehouseDBContext: DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product
            .Property(e => e.Id)
            .HasConversion(
                typed => typed.Value,
                plain => new ProductId(plain)
            );

        product
            .OwnsOne(e => e.Sku);
    }
}

public partial class Program
{
}
