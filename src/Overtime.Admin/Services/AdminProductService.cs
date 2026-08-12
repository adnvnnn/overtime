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
        IQueryable<Product> q = db.Products.Include(p => p.Sizes).Include(p => p.Brand);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(p => EF.Functions.Like(p.Name, pattern) || EF.Functions.Like(p.Brand!.Name, pattern) || EF.Functions.Like(p.Category, pattern));
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
            .Include(p => p.Brand)
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

        foreach (var size in product.Sizes)
            size.ProductId = id;

        db.Products.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existing is null)
            return;

        existing.Name = product.Name;
        existing.BrandId = product.BrandId;
        existing.Category = product.Category;
        existing.Usage = product.Usage;
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

    public async Task UpdateSizesAsync(string productId, List<ProductSize> sizes)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.ProductSizes.Where(s => s.ProductId == productId).ToListAsync();
        var selectedBySize = sizes.ToDictionary(s => s.Size);

        foreach (var row in existing)
        {
            if (selectedBySize.Remove(row.Size, out var selected))
                row.StockQty = Math.Max(0, selected.StockQty);
            else
                db.ProductSizes.Remove(row);
        }

        foreach (var newSize in selectedBySize.Values)
            db.ProductSizes.Add(new ProductSize { ProductId = productId, Size = newSize.Size, StockQty = Math.Max(0, newSize.StockQty) });

        await db.SaveChangesAsync();
    }

    private static string Slugify(string name)
    {
        var slug = NonAlphaNumeric().Replace(name.Trim().ToLowerInvariant(), "-").Trim('-');
        return string.IsNullOrEmpty(slug) ? "product" : slug;
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();
}
