using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Overtime.Data;
using Overtime.Data.Entities;
using Overtime.Web.Resources;

namespace Overtime.Web.Services;

public partial class OtpService(IDbContextFactory<AppDbContext> dbFactory, IStringLocalizer<AppStrings> localizer)
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
            return (false, localizer["Otp_TooSoon"], null);

        var code = Random.Shared.Next(100_000, 999_999).ToString();
        db.OtpCodes.Add(new OtpCode
        {
            PhoneNumber = phone,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(CodeLifetime),
        });
        await db.SaveChangesAsync();

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
            return (false, localizer["Otp_NotRequested"], null);

        if (otp.ExpiresAt < DateTime.UtcNow)
            return (false, localizer["Otp_Expired"], null);

        if (otp.Attempts >= MaxAttempts)
            return (false, localizer["Otp_MaxAttemptsReached"], null);

        if (otp.Code != code)
        {
            otp.Attempts++;
            await db.SaveChangesAsync();
            return (false, localizer["Otp_IncorrectCode"], null);
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
