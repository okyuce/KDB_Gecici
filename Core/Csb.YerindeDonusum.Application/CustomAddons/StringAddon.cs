using CSB.Core.Extensions;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Csb.YerindeDonusum.Application.CustomAddons;

internal static class StringAddon
{
    public static string ToSlugUrl(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var turkceKarakterler = new Dictionary<string, string>()
        {
            { "Ç", "c" },
            { "Ş", "s" },
            { "Ğ", "g" },
            { "Ü", "u" },
            { "Ö", "o" },
            { "İ", "i" },
            { "ç", "c" },
            { "ş", "s" },
            { "ğ", "g" },
            { "ü", "u" },
            { "ö", "o" },
            { "ı", "i" },
            { "I", "i" },
        };

        foreach (var turkceKarakter in turkceKarakterler)
            text = text.Replace(turkceKarakter.Key, turkceKarakter.Value);

        Regex r = new Regex("[^a-zA-Z0-9]");
        text = r.Replace(text, "");

        return text.Trim().ToLower(new System.Globalization.CultureInfo("en-EN")); // "I" karakterini "ı" olarak çevirmesin vb. diye culture olarak "en-EN" eklendi
    }

    public static string ToClearPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return phone;

        return new String(phone.Where(Char.IsDigit).ToArray()).Trim().TrimStart('0');
    }

    public static bool ValidatePhone(string phone)
    {
        return ToClearPhone(phone).Length >= 10;
    }

    public static bool ValidateEmail(string email)
    {
        try
        {
            var emailAddress = new MailAddress(email);
            return emailAddress.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }

    public static string ToMaskedWord(string? word, int startVisibleLength = 2)
    {
        if (string.IsNullOrWhiteSpace(word))
            return "";

        var wordList = new List<string>();
        foreach (var w in word.Trim().Split(' '))
        {
            if (w.Length > startVisibleLength)
                wordList.Add(w.Substring(0, startVisibleLength).PadRight(w.Length, '*'));
            else
                wordList.Add(w);
        }

        return string.Join(" ", wordList);
    }

    public static string ToMaskedWord(long? number, int startVisibleLength = 2)
    {
        var word = number?.ToString();

        if (string.IsNullOrWhiteSpace(word))
            return "";

        if (word.Length > startVisibleLength)
            return word.Substring(0, startVisibleLength).PadRight(word.Length, '*');
        else
            return word;
    }

    public static bool IsOnlySpecialChars(string? str)
    {
        if (string.IsNullOrWhiteSpace(str)) return false;

        string result = Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

        if (result.Trim().Length > 0)
            return false;

        return true;
    }
}