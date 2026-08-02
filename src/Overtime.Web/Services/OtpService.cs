using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public partial class OtpService(IDbContextFactory<AppDbContext> dbFactory, ILogger<OtpService> logger)
{
    private static readonly TimeSpan CodeLifetime = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan ResendCooldown = TimeSpan.FromSeconds(60);
    private const int MaxAttempts = 5;

    public static string NormalizePhone(string input)
    {
        var normalized = input.Trim().Select(c => c switch
        {
            >= '۰' and <= '۹' => (char)(c - '۰' + '0'),
            >= '٠' and <= '٩' => (char)(c - '٠' + '0'),
            _ => c,
        });
        return new string([.. normalized.Where(c => !char.IsWhiteSpace(c) && c != '-')]);
    }

    public static bool IsValidPhone(string phone) => PhoneRegex().IsMatch(phone);

    public async Task<(bool Ok, string? Error, string? DebugCode)> RequestCodeAsync(string phone)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var recent = await db.OtpCodes
            .Where(o => o.PhoneNumber == phone)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (recent is not null && DateTime.UtcNow - recent.CreatedAt < ResendCooldown)
            return (false, "لطفاً کمی صبر کنید و دوباره تلاش کنید.", null);

        var code = Random.Shared.Next(100_000, 999_999).ToString();
        db.OtpCodes.Add(new OtpCode
        {
            PhoneNumber = phone,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(CodeLifetime),
        });
        await db.SaveChangesAsync();

        // No SMS gateway is wired up yet, so the code is only logged for now.
        logger.LogInformation("OTP code for {Phone}: {Code}", phone, code);

        return (true, null, code);
    }

    public async Task<(bool Ok, string? Error, User? User)> VerifyCodeAsync(string phone, string code)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var otp = await db.OtpCodes
            .Where(o => o.PhoneNumber == phone && o.ConsumedAt == null)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();

        if (otp is null)
            return (false, "ابتدا کد تایید را درخواست کنید.", null);

        if (otp.ExpiresAt < DateTime.UtcNow)
            return (false, "کد تایید منقضی شده است. دوباره درخواست کنید.", null);

        if (otp.Attempts >= MaxAttempts)
            return (false, "تعداد تلاش‌های مجاز به پایان رسید. کد جدید درخواست کنید.", null);

        if (otp.Code != code)
        {
            otp.Attempts++;
            await db.SaveChangesAsync();
            return (false, "کد وارد شده صحیح نیست.", null);
        }

        otp.ConsumedAt = DateTime.UtcNow;

        var user = await db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
        if (user is null)
        {
            user = new User { PhoneNumber = phone, CreatedAt = DateTime.UtcNow };
            db.Users.Add(user);
        }

        await db.SaveChangesAsync();
        return (true, null, user);
    }

    [GeneratedRegex("^09\\d{9}$")]
    private static partial Regex PhoneRegex();
}
