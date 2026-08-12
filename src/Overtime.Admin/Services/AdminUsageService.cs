using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminUsageService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Usage>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Usages.OrderBy(u => u.DisplayOrder).ToListAsync();
    }
}
