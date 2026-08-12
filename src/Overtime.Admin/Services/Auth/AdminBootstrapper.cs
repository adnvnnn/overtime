using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public static class AdminBootstrapper
{
    /// <summary>
    /// Creates the first admin account from config (Admin:Username / Admin:Password / Admin:OwnerName)
    /// the first time the app runs against an AdminUsers table with no rows. Once at least one admin
    /// exists, accounts are managed entirely from the "مدیریت ادمین‌ها" page and this is a no-op.
    /// </summary>
    public static async Task SeedIfEmptyAsync(AppDbContext db, IConfiguration configuration)
    {
        if (await db.AdminUsers.AnyAsync())
            return;

        var password = configuration["Admin:Password"];
        if (string.IsNullOrEmpty(password))
            return;

        db.AdminUsers.Add(new AdminUser
        {
            Username = configuration["Admin:Username"] ?? "admin",
            PasswordHash = PasswordHasher.Hash(password),
            FullName = configuration["Admin:OwnerName"],
            CreatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
    }
}
