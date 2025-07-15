namespace Csb.YerindeDonusum.Application.CustomAddons;

internal static class HasarTespitAddon
{
    public static string AskiKoduToUpper(string? askiKodu)
    {
        if (string.IsNullOrWhiteSpace(askiKodu)) return "";

        var turkceKarakterler = new Dictionary<string, string>()
        {
            { "Ç", "C" },
            { "Ş", "S" },
            { "Ğ", "G" },
            { "Ü", "U" },
            { "Ö", "O" },
            { "İ", "I" },
            { "ç", "C" },
            { "ş", "S" },
            { "ğ", "G" },
            { "ü", "U" },
            { "ö", "O" },
            { "ı", "I" },
        };

        foreach (var turkceKarakter in turkceKarakterler)
            askiKodu = askiKodu.Replace(turkceKarakter.Key, turkceKarakter.Value);

        return askiKodu.Trim().ToUpper();
    }
}