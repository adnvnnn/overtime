using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class ProductService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Product>> GetCatalogAsync(string categorySlug = "all", string? query = null, string sort = "default",
        long? minPrice = null, long? maxPrice = null, bool inStockOnly = false, string? brandSlug = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        IQueryable<Product> q = db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Include(p => p.Usages);

        if (categorySlug != "all")
            q = q.Where(p => p.Categories.Any(c => c.Slug == categorySlug));

        if (!string.IsNullOrWhiteSpace(brandSlug))
            q = q.Where(p => p.Brand!.Slug == brandSlug);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(p => EF.Functions.Like(p.Name, pattern) || p.Categories.Any(c => EF.Functions.Like(c.Name, pattern)) || EF.Functions.Like(p.Brand!.Name, pattern));
        }

        if (minPrice is not null)
            q = q.Where(p => p.Price >= minPrice.Value);

        if (maxPrice is not null)
            q = q.Where(p => p.Price <= maxPrice.Value);

        if (inStockOnly)
            q = q.Where(p => p.Sizes.Any(s => s.StockQty > 0));

        q = sort switch
        {
            "price-asc" => q.OrderBy(p => p.Price),
            "price-desc" => q.OrderByDescending(p => p.Price),
            "name" => q.OrderBy(p => p.Name),
            _ => q.OrderBy(p => p.DisplayOrder),
        };

        return await q.ToListAsync();
    }

    public async Task<int> CountAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products.CountAsync();
    }

    public async Task<(long Min, long Max)> GetPriceBoundsAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        if (!await db.Products.AnyAsync())
            return (0, 0);

        return (await db.Products.MinAsync(p => p.Price), await db.Products.MaxAsync(p => p.Price));
    }

    public async Task<Product?> GetBySlugAsync(string slug)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Specs.OrderBy(s => s.SortOrder))
            .Include(p => p.Sizes.OrderBy(s => s.Size))
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Include(p => p.Usages)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<List<Product>> GetTopAsync(int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Include(p => p.Usages)
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Product>> GetRelatedAsync(int excludeId, int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Brand)
            .Include(p => p.Categories)
            .Include(p => p.Usages)
            .Where(p => p.Id != excludeId)
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();
    }
}
