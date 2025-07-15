using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using System.Text;
using System.Text.RegularExpressions;

namespace Csb.YerindeDonusum.Application.Extensions;

/// <summary>
/// Mehmet SÜMER
/// FluentValidation kullanırken propertyleri daha esnek bir şekilde validate etmeyi sağlayan sınıf.
/// .NotEmpty() kullanıldığında Id alanlarında 0 gelebiliyordu, 
/// bu istenmediğinden kendi NotEmpty metotlarımız ile > 0 kontrolü de eklendi.
/// Kullanım:
/// .Must(FluentValidationExtension.NotEmpty)
/// </summary>
public static class FluentValidationExtension
{
    #region NotNull, NotEmpty, NotWhiteSpace Validations (integerlar için null ve 0 - stringler için null ve "" EMPTY sayilir.)
    // Id' ler null ve 0 olamayacağı için burada NotEmpty kullanacağız.
    public static bool NotEmpty(int? val)
    {
        return val != null && val > 0;
    }
    public static bool NotEmpty(double? val)
    {
        return val != null && val > 0;
    }
    public static bool NotEmpty(bool? val)
    {
        return val != null;
    }
    public static bool NotEmpty(short? val)
    {
        return val != null && val > 0;
    }
    public static bool NotEmpty<T>(List<T>? val)
    {
        return val != null && val.Count > 0;
    }
    public static bool NotEmpty(decimal? val)
    {
        return val != null && val > 0;
    }
    public static bool NotEmpty(int val)
    {
        return val > 0;
    }
    public static bool NotEmpty(long? val)
    {
        return val != null && val > 0;
    }
    public static bool NotEmpty(long val)
    {
        return val > 0;
    }
    public static bool NotEmpty(string? val)
    {
        return !string.IsNullOrEmpty(val);
    }
    public static bool NotEmpty(Guid? val)
    {
        return val != null && val.HasValue;
    }
    public static bool NotEmpty(DateTime? val)
    {
        return val != null && val.HasValue;
    }
    public static bool NotEmpty(DateOnly? val)
    {
        return val != null && val.HasValue;
    }
    public static bool NotEmpty(DosyaDto? val)
    {
        return NotWhiteSpace(val?.DosyaBase64) && NotWhiteSpace(val?.DosyaUzanti);
    }
    public static bool IsEmpty(DosyaDto? val)
    {
        return !(NotWhiteSpace(val?.DosyaBase64) && NotWhiteSpace(val?.DosyaUzanti));
    }

    public static bool NotWhiteSpace(string? val)
    {
        return !string.IsNullOrWhiteSpace(val);

    }

    public static bool NotNull(int? val)
    {
        return val != null;
    }
    public static bool NotNull(decimal? val)
    {
        return val != null;
    }
    public static bool NotNull(string val)
    {
        return val != null;
    }
    public static bool NotNull(DateTime? val)
    {
        return val != null;
    }
    public static bool JsonNotEmpty(string? val)
    {
        return NotEmpty(val) && val != "{}" && val != "[]";
    }
    #endregion

    public static bool InclusiveBetween(int? val, int from = int.MinValue, int to = int.MaxValue)
    {
        return val != null && val >= from && val <= to;
    }
    public static bool ExclusiveBetween(int? val, int from = int.MinValue, int to = int.MaxValue)
    {
        return val != null && val > from && val < to;
    }

    public static bool TcDogrula(long? TCNO)
    {
        return TcDogrula(TCNO?.ToString());
    }

    public static bool TcDogrula(string? TCNO)
    {
        try
        {
            if (TCNO?.Length != 11)
                return false;
            else if (TCNO.Substring(0, 1) == "0")
                return false;

            int toplam1 = Convert.ToInt32(TCNO[0].ToString()) + Convert.ToInt32(TCNO[2].ToString()) + Convert.ToInt32(TCNO[4].ToString()) + Convert.ToInt32(TCNO[6].ToString()) + Convert.ToInt32(TCNO[8].ToString());
            int toplam2 = Convert.ToInt32(TCNO[1].ToString()) + Convert.ToInt32(TCNO[3].ToString()) + Convert.ToInt32(TCNO[5].ToString()) + Convert.ToInt32(TCNO[7].ToString());

            int sonuc = ((toplam1 * 7) - toplam2) % 10;

            if (sonuc < 0)
                sonuc += 10;

            if (sonuc.ToString() != TCNO[9].ToString())
                return false;

            int toplam3 = 0;
            for (int i = 0; i < 10; i++)
                toplam3 += Convert.ToInt32(TCNO[i].ToString());

            return (toplam3 % 10).ToString() == TCNO[10].ToString();
        }
        catch { }

        return false;
    }

    public static bool IbanDogrula(string? IBAN)
    {
        try
        {
            IBAN = IBAN?.ToUpper()?.Replace("-", string.Empty);

            if (!NotWhiteSpace(IBAN)) return false;

            if (Regex.IsMatch(IBAN, "^[A-Z0-9]"))
            {
                IBAN = IBAN.Replace(" ", string.Empty);
                string bank = IBAN.Substring(4, IBAN.Length - 4) + IBAN.Substring(0, 4);
                int asciiShift = 55;
                StringBuilder sb = new StringBuilder();
                foreach (char c in bank)
                {
                    int v;
                    if (Char.IsLetter(c)) v = c - asciiShift;
                    else v = int.Parse(c.ToString());
                    sb.Append(v);
                }

                string checkSumString = sb.ToString();
                int checksum = int.Parse(checkSumString.Substring(0, 1));
                for (int i = 1; i < checkSumString.Length; i++)
                {
                    int v = int.Parse(checkSumString.Substring(i, 1));
                    checksum *= 10;
                    checksum += v;
                    checksum %= 97;
                }

                return checksum == 1;
            }
        }
        catch
        {
        }

        return false;
    }

    public static bool IsStringGuid(string? val)
    {
        return Guid.TryParse(val, out Guid appealGuid);
    }

    public static bool ValidatePhone(string? val)
    {
        return StringAddon.ValidatePhone(val);
    }

    public static bool ValidateEmail(string? val)
    {
        return StringAddon.ValidateEmail(val);
    }
    public static bool NotOnlySpecialChars(string? val)
    {
        return !StringAddon.IsOnlySpecialChars(val);
    }

    public static bool AskiKoduDogrula(string? askiKodu)
    {
        try
        {
            askiKodu = HasarTespitAddon.AskiKoduToUpper(askiKodu);

            if (!NotWhiteSpace(askiKodu))
                return false;

            return Regex.IsMatch(askiKodu, @"[A-Z0-9/]");
        }
        catch
        {
            return false;
        }
    }
}