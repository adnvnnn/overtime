using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminAccountService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<AdminUser>> GetAllAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.AdminUsers.OrderBy(a => a.Username).ToListAsync();
    }

    public async Task<AdminUser?> VerifyLoginAsync(string username, string password)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var admin = await db.AdminUsers.FirstOrDefaultAsync(a => a.Username == username.Trim());
        if (admin is null || !PasswordHasher.Verify(password, admin.PasswordHash))
            return null;

        return admin;
    }

    public async Task<(bool Ok, string? Error)> CreateAsync(string username, string password, string? fullName)
    {
        username = username.Trim();
        await using var db = await dbFactory.CreateDbContextAsync();
        if (await db.AdminUsers.AnyAsync(a => a.Username == username))
            return (false, "این نام کاربری قبلاً استفاده شده است.");

        db.AdminUsers.Add(new AdminUser
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            FullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName.Trim(),
            CreatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
        return (true, null);
    }

    public async Task ResetPasswordAsync(int id, string newPassword)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var admin = await db.AdminUsers.FirstOrDefaultAsync(a => a.Id == id);
        if (admin is null)
            return;

        admin.PasswordHash = PasswordHasher.Hash(newPassword);
        await db.SaveChangesAsync();
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var count = await db.AdminUsers.CountAsync();
        if (count <= 1)
            return (false, "امکان حذف آخرین ادمین وجود ندارد.");

        await db.AdminUsers.Where(a => a.Id == id).ExecuteDeleteAsync();
        return (true, null);
    }
}
