using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CustomAddons;
public class UserPasswordGenerator
{
    // Regex kurallarını tanımlayalım
    private static readonly Regex UpperCaseRegex = new Regex(@"[A-Z]");
    private static readonly Regex LowerCaseRegex = new Regex(@"[a-z]");
    private static readonly Regex DigitRegex = new Regex(@"[0-9]");
    private static readonly Regex SpecialCharRegex = new Regex(@"[^a-zA-Z0-9]");

    public static string GeneratePassword(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("Şifre en az 8 karakter olmalıdır.");

        // Karakter setleri
        string lowerCase = "abcdefghijkmnopqrstuvwxyz";
        string upperCase = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
        string digits = "0123456789";
        string specialCharacters = "!@#*-.$?";

        // Tüm karakterlerin birleşimi
        string allCharacters = lowerCase + upperCase + digits + specialCharacters;

        Random rand = new Random();
        StringBuilder password = new StringBuilder();

        // Şifreye her tür karakteri ekleyeceğiz: küçük harf, büyük harf, rakam, özel karakter
        password.Append(lowerCase[rand.Next(lowerCase.Length)]);
        password.Append(upperCase[rand.Next(upperCase.Length)]);
        password.Append(digits[rand.Next(digits.Length)]);
        password.Append(specialCharacters[rand.Next(specialCharacters.Length)]);

        // Kalan karakterleri rastgele seçerek ekleyelim
        for (int i = password.Length; i < length; i++)
        {
            password.Append(allCharacters[rand.Next(allCharacters.Length)]);
        }

        // Şifreyi karıştırmak (karakterler yer değiştiriyor)
        var shuffledPassword = password.ToString().ToCharArray();
        rand.Shuffle(shuffledPassword);

        // Şifreyi string'e çevir
        string generatedPassword = new string(shuffledPassword);

        // Regex ile şifreyi kontrol et
        if (!IsValidPassword(generatedPassword))
        {
            return GeneratePassword(length);  // Eğer şifre kurallara uymuyorsa yeniden üret
        }

        return generatedPassword;
    }

    // Şifreyi kontrol eden metot
    private static bool IsValidPassword(string password)
    {
        return UpperCaseRegex.IsMatch(password) &&
               LowerCaseRegex.IsMatch(password) &&
               DigitRegex.IsMatch(password) &&
               SpecialCharRegex.IsMatch(password) &&
               !ContainsConsecutiveRepeatingChars(password);
    }

    // Ardışık tekrar karakterler kontrolü
    private static bool ContainsConsecutiveRepeatingChars(string password)
    {
        for (int i = 1; i < password.Length; i++)
        {
            if (password[i] == password[i - 1])
                return true;
        }
        return false;
    }
}

public static class RandomExtensions
{
    // Karakter dizisini karıştırmak için yardımcı metod
    public static void Shuffle(this Random rng, char[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            char value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}

