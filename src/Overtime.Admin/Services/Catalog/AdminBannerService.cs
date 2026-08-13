using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminBannerService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Banner>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Banners.OrderBy(b => b.Kind).ThenBy(b => b.DisplayOrder).ToListAsync();
    }

    public async Task<(bool Ok, string? Error)> CreateAsync(Banner banner)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.Banners.Add(banner);
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Ok, string? Error)> UpdateAsync(Banner banner)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.Banners.FirstOrDefaultAsync(b => b.Id == banner.Id);
        if (existing is null)
            return (false, "بنر یافت نشد.");

        existing.Kind = banner.Kind;
        existing.ImageUrl = banner.ImageUrl;
        existing.Badge = banner.Badge;
        existing.Title = banner.Title;
        existing.Price = banner.Price;
        existing.LinkUrl = banner.LinkUrl;
        existing.DisplayOrder = banner.DisplayOrder;
        existing.IsActive = banner.IsActive;
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        await db.Banners.Where(b => b.Id == id).ExecuteDeleteAsync();
        return (true, null);
    }
}
