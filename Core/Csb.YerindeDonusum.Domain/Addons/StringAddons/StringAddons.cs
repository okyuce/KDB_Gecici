using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Domain.Addons.StringAddons;

public static class StringAddons
{
    public static string GenerateAppeadlCode(this string value)
    {
        var rnd = new Random();
        string alphabet = "ABCDEFGHIJKLMNOPRSTUVYZ";
        string numbericData = "0123456789";
        var sb = new StringBuilder(); ;

        for (int i = 0; i < 3; i++)
        {
            sb.Append(alphabet[rnd.Next(alphabet.Length)]);
        }

        sb.Append("-");

        for (int i = 0; i < 9; i++)
        {
            sb.Append(numbericData[rnd.Next(numbericData.Length)]);
        }

        return sb.ToString();
    }

    public static bool Search(string? haystack, string? needle)
    {
        if (string.IsNullOrWhiteSpace(haystack) || string.IsNullOrWhiteSpace(needle)) return true;

        haystack = NormalizeText(haystack);
        needle = NormalizeText(needle);

        return haystack.Contains(needle);
    }

	/// <summary>
	/// String'i normalize eder. Türkçe karakterleri ingilizce' ye çevirir. Örneğin;
	/// girdi: 1. Yerinde dönüşüm projesinde kırmızı çizgiler nelerdir?
    /// çıktı: 1. yerinde donusum projesinde kirmizi cizgiler nelerdir?
    /// Böylece arama yapılırken donusum de yazılsa, dönüşüm de yazılsa ilgili sonuç dönecektir.
	/// </summary>
	/// <param name="text"></param>
	/// <returns>string</returns>
	public static string NormalizeText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";

        return string.Join("", text.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                                    .ToLower().Trim().Replace("ı", "i");
    }

    /// <summary>
    /// Gelen Stringdeki HTML Taglarını siler.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
	public static string StripHTML(string? input)
	{
        if (string.IsNullOrWhiteSpace(input)) return "";

		return Regex.Replace(input, "<.*?>", String.Empty);
	}

}
