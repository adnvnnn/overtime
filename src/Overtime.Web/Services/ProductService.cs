using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class ProductService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Product>> GetCatalogAsync(string category = "all", string? query = null, string sort = "default")
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        IQueryable<Product> q = db.Products.Include(p => p.Images.OrderBy(i => i.SortOrder));

        if (category != "all")
            q = q.Where(p => p.Category.Contains(category));

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(p => EF.Functions.Like(p.Name, pattern) || EF.Functions.Like(p.Category, pattern));
        }

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

    public async Task<Product?> GetByIdAsync(string id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Include(p => p.Specs.OrderBy(s => s.SortOrder))
            .Include(p => p.Sizes.OrderBy(s => s.Size))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetTopAsync(int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Product>> GetRelatedAsync(string excludeId, int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Products
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Where(p => p.Id != excludeId)
            .OrderBy(p => p.DisplayOrder)
            .Take(count)
            .ToListAsync();
    }
}
