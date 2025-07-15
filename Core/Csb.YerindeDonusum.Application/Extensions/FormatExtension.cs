using System.Globalization;

namespace Csb.YerindeDonusum.Application.Extensions;

public static class FormatExtension
{
    private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");

    #region Currency Formats
    public static string ToTurkishLira(this string? src)
    {
        return string.Format("{0:C} ₺", src);
    }
    public static string ToTurkishLira(this double? src, bool hideIsZeroOrThanLess = false)
    {
        if (!(src > 0) && hideIsZeroOrThanLess)
            return "";
        return src?.ToString("C", TurkishCulture) ?? "";
    }
    public static string ToTurkishLira(this decimal src, int decimalNumber = 2, bool hideIsZeroOrThanLess = false)
    {
        if (!(src > 0) && hideIsZeroOrThanLess)
            return "";
        return src.ToString($"N{decimalNumber}", TurkishCulture) + " ₺";
    }
    public static string ToTurkishLira(this decimal? src, int decimalNumber = 2, bool hideIsZeroOrThanLess = false)
    {
        if (!(src > 0) && hideIsZeroOrThanLess)
            return "";
        return src?.ToString($"N{decimalNumber}", TurkishCulture) + " ₺";
    }
    public static string ToTurkishLira(this float? src)
    {
        //return string.Format("{0:C} ₺", src);
        return src?.ToString("C", TurkishCulture) ?? "";
    }
    public static double TurkishLiraToNumber(this string src)
    {
        double.TryParse(src, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("tr-TR"), out var parsed);
        return parsed;
    }
    #endregion

    #region DateTime Formats
    public static string ToDateString(this DateTime src)
    {
        return src.ToString("dd/MM/yyyy");
    }
    public static string ToDateTimeString(this DateTime src)
    {
        return src.ToString("dd/MM/yyyy HH:mm");
    }
    public static string ToDateString(this DateTime? src)
    {
        return src?.ToString("dd/MM/yyyy") ?? "";
    }
    public static string ToDateTimeString(this DateTime? src)
    {
        return src?.ToString("dd/MM/yyyy HH:mm") ?? "";
    }

    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day).Subtract(new TimeSpan(0, 0, 0, 0, 0));
    }
    public static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    public static DateTime StartOfDay(this DateTime? date)
    {
        date ??= DateTime.Now;
        return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day).Subtract(new TimeSpan(0, 0, 0, 0, 0));
    }
    public static DateTime EndOfDay(this DateTime? date)
    {
        date ??= DateTime.Now;
        return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }

    #endregion
}