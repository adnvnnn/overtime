using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminCategoryService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Category>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
    }
}
