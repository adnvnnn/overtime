using System.Globalization;

namespace Overtime.Admin.Services;

public static class PersianNumberFormatter
{
    private static readonly char[] FaDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    public static string ToFa(this long input) => ToFa(input.ToString("N0", CultureInfo.InvariantCulture));

    public static string ToFa(this int input) => ToFa(input.ToString(CultureInfo.InvariantCulture));

    public static string ToFa(this string input)
    {
        var chars = input.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
        {
            if (chars[i] is >= '0' and <= '9')
                chars[i] = FaDigits[chars[i] - '0'];
        }
        return new string(chars);
    }

    public static string FormatToman(this long price) => $"{ToFa(price)} تومان";
}
