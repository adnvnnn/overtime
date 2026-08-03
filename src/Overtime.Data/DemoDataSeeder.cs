using Microsoft.EntityFrameworkCore;

namespace Overtime.Data;

public static class DemoDataSeeder
{
    /// <summary>
    /// Seeds demo customers and orders once, the first time the app runs against an
    /// empty Orders table. Unlike the product catalog, this data isn't baked into a
    /// migration: Users/Orders grow from real usage, and a migration with fixed
    /// primary keys would collide with rows created by actual signups.
    /// </summary>
    public static async Task SeedIfEmptyAsync(AppDbContext db)
    {
        if (await db.Orders.AnyAsync())
            return;

        var existingPhones = await db.Users.Select(u => u.PhoneNumber).ToListAsync();
        var newUsers = SeedData.GetDemoUsers().Where(u => !existingPhones.Contains(u.PhoneNumber)).ToList();
        if (newUsers.Count > 0)
            db.Users.AddRange(newUsers);

        db.Orders.AddRange(SeedData.GetDemoOrders());

        await db.SaveChangesAsync();
    }
}
