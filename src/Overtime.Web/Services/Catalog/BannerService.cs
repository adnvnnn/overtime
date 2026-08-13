using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class BannerService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Banner>> GetActiveAsync(BannerKind kind)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Banners
            .Where(b => b.Kind == kind && b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();
    }
}
