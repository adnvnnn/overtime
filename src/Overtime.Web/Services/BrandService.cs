using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class BrandService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Brand>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Brands.OrderBy(b => b.DisplayOrder).ToListAsync();
    }
}
