using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class CategoryService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Category>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
    }
}
