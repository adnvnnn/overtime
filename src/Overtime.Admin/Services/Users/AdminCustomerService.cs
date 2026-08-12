using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminCustomerService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<User>> GetAllAsync(string? query = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        IQueryable<User> q = db.Users;

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(u => EF.Functions.Like(u.PhoneNumber, pattern));
        }

        return await q.OrderByDescending(u => u.CreatedAt).ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Users.CountAsync();
    }
}
