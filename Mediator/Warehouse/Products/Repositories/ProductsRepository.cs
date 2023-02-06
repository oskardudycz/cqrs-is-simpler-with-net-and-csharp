using Microsoft.EntityFrameworkCore;
using Warehouse.Storage;

namespace Warehouse.Products;

internal interface IProductRepository
{
    ValueTask<Product?> Find(ProductId productId, CancellationToken ct);


    Task Add(Product product, CancellationToken ct);


    Task Update(Product product, CancellationToken ct);


    Task Delete(Product product, CancellationToken ct);

    ValueTask<bool> ProductWithSKUExists(SKU productSKU, CancellationToken ct);

    ValueTask<ProductDetails?> GetProductDetails(ProductId productId, CancellationToken ct);

    ValueTask<IReadOnlyList<ProductListItem>> GetProducts(
        string? filter,
        int page,
        int pageSize,
        CancellationToken ct
    );
}

internal class ProductsRepository: IProductRepository
{
    private readonly WarehouseDBContext dbContext;

    public ProductsRepository(WarehouseDBContext dbContext) =>
        this.dbContext = dbContext;

    public ValueTask<bool> ProductWithSKUExists(
        SKU productSKU,
        CancellationToken ct
    ) =>
        new(
            dbContext.Set<Product>().AnyAsync(
                product => product.Sku.Value == productSKU.Value, ct
            ));

    public ValueTask<Product?> Find(ProductId productId, CancellationToken ct) =>
        dbContext.FindAsync<Product>(new[] { productId }, ct);

    public async Task Add(Product product, CancellationToken ct)
    {
        await dbContext.AddAsync(product, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task Update(Product product, CancellationToken ct)
    {
        dbContext.Update(product);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task Delete(Product product, CancellationToken ct)
    {
        dbContext.Update(product);
        await dbContext.SaveChangesAsync(ct);
    }

    public async ValueTask<ProductDetails?> GetProductDetails(ProductId productId, CancellationToken ct)
    {
        var product = await dbContext.Set<Product>().AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == productId, ct);

        if (product == null)
            return null;

        return new ProductDetails(
            product.Id.Value,
            product.Sku.Value,
            product.Name,
            product.Description
        );
    }

    public async ValueTask<IReadOnlyList<ProductListItem>> GetProducts(
        string? filter,
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var products = dbContext.Set<Product>().AsNoTracking();

        var filteredProducts = string.IsNullOrEmpty(filter)
            ? products
            : products
                .Where(p =>
                    p.Sku.Value.Contains(filter!) ||
                    p.Name.Contains(filter!) ||
                    p.Description!.Contains(filter!)
                );

        var result = await filteredProducts
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .ToListAsync(ct);

        var sth = result
            .Select(p => new ProductListItem(p.Id.Value, p.Sku.Value, p.Name))
            .ToList();

        return sth;
    }
}
