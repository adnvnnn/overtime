using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public partial class AdminProductService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Product>> GetAllAsync(string? query = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        IQueryable<Product> q = db.Products.Include(p => p.Sizes);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(p => EF.Functions.Like(p.Name, pattern) || EF.Functions.Like(p.Category, pattern));
        }

        return await q.OrderBy(p => p.DisplayOrder).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Specs.OrderBy(s => s.SortOrder))
            .Include(p => p.Sizes.OrderBy(s => s.Size))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var baseId = Slugify(product.Name);
        var id = baseId;
        var suffix = 2;
        while (await db.Products.AnyAsync(p => p.Id == id))
            id = $"{baseId}-{suffix++}";
        product.Id = id;

        if (product.Sizes.Count == 0)
        {
            foreach (var size in DefaultSizes)
                product.Sizes.Add(new ProductSize { ProductId = id, Size = size, StockQty = 0 });
        }

        db.Products.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    private static readonly int[] DefaultSizes = [40, 41, 42, 43, 44, 45];

    public async Task UpdateAsync(Product product)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existing is null)
            return;

        existing.Name = product.Name;
        existing.Category = product.Category;
        existing.Price = product.Price;
        existing.Description = product.Description;
        existing.Badge = product.Badge;
        existing.DisplayOrder = product.DisplayOrder;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        await db.Products.Where(p => p.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateStockAsync(string productId, int size, int stockQty)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var row = await db.ProductSizes.FirstOrDefaultAsync(s => s.ProductId == productId && s.Size == size);
        if (row is not null)
        {
            row.StockQty = Math.Max(0, stockQty);
            await db.SaveChangesAsync();
        }
    }

    private static string Slugify(string name)
    {
        var slug = NonAlphaNumeric().Replace(name.Trim().ToLowerInvariant(), "-").Trim('-');
        return string.IsNullOrEmpty(slug) ? "product" : slug;
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();
}
