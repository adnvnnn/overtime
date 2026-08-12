using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public partial class AdminBrandService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Brand>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Brands.Include(b => b.Products).OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name).ToListAsync();
    }

    public async Task<(bool Ok, string? Error)> CreateAsync(Brand brand)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var slug = Slugify(string.IsNullOrWhiteSpace(brand.Slug) ? brand.Name : brand.Slug);
        if (await db.Brands.AnyAsync(b => b.Slug == slug))
            return (false, "این نامک قبلاً استفاده شده است.");

        brand.Slug = slug;
        db.Brands.Add(brand);
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Ok, string? Error)> UpdateAsync(Brand brand)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);
        if (existing is null)
            return (false, "برند یافت نشد.");

        var slug = Slugify(string.IsNullOrWhiteSpace(brand.Slug) ? brand.Name : brand.Slug);
        if (slug != existing.Slug && await db.Brands.AnyAsync(b => b.Slug == slug))
            return (false, "این نامک قبلاً استفاده شده است.");

        existing.Name = brand.Name;
        existing.Slug = slug;
        existing.LogoUrl = brand.LogoUrl;
        existing.DisplayOrder = brand.DisplayOrder;
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var inUse = await db.Products.AnyAsync(p => p.BrandId == id);
        if (inUse)
            return (false, "این برند به محصولاتی متصل است و قابل حذف نیست.");

        await db.Brands.Where(b => b.Id == id).ExecuteDeleteAsync();
        return (true, null);
    }

    private static string Slugify(string name)
    {
        var slug = NonAlphaNumeric().Replace(name.Trim().ToLowerInvariant(), "-").Trim('-');
        return string.IsNullOrEmpty(slug) ? "brand" : slug;
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();
}
