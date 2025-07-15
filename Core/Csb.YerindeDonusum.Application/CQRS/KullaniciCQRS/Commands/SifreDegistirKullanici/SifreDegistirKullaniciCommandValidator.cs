using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.SifreDegistirKullanici;

public class SifreDegistirKullaniciCommandValidator : AbstractValidator<SifreDegistirKullaniciCommand>
{
    public SifreDegistirKullaniciCommandValidator()
    {
        RuleFor(x => x.YeniSifre)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches(@"[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.")
            .Must(BeValidPassword).WithMessage("Şifre ardışık tekrar karakterler içeremez.");

        RuleFor(x => x.YeniSifre)
        .Must((x, YeniSifre) => YeniSifre == x.YeniSifreYeniden)
            .WithMessage("Yeni şifre bilgileri eşleşmemektedir.");

    }
    private bool BeValidPassword(string password)
    {
        // Ardışık 3 aynı karakteri kontrol etmek için regex kullanılır
        var pattern = @"(.)\1\1";
        if (Regex.IsMatch(password, pattern))
        {
            return false; // 3 aynı karakter peş peşe varsa, geçersiz kabul edilir
        }

        // Ardışık rakamlar ve harfler için kontrol
        for (int i = 0; i < password.Length - 2; i++)
        {
            // Sayıların ardışıklığını kontrol et
            if (char.IsDigit(password[i]) && char.IsDigit(password[i + 1]) && char.IsDigit(password[i + 2]))
            {
                int first = int.Parse(password[i].ToString());
                int second = int.Parse(password[i + 1].ToString());
                int third = int.Parse(password[i + 2].ToString());

                // Ardışık sayılar kontrolü: (e.g. 123, 234, 345 vb.)
                if (second == first + 1 && third == second + 1)
                {
                    return false; // Ardışık sayılar varsa geçersiz kabul edilir
                }
            }

            // Harflerin ardışıklığını kontrol et (a, b, c, ... veya A, B, C, ...)
            if (char.IsLetter(password[i]) && char.IsLetter(password[i + 1]) && char.IsLetter(password[i + 2]))
            {
                char first = password[i];
                char second = password[i + 1];
                char third = password[i + 2];

                // Ardışık harfler kontrolü (a, b, c veya A, B, C gibi)
                if (char.ToLower(second) == char.ToLower(first) + 1 && char.ToLower(third) == char.ToLower(second) + 1)
                {
                    return false; // Ardışık harfler varsa geçersiz kabul edilir
                }
            }
        }

        return true; // Ardışık sayılar veya harfler yoksa geçerli kabul edilir
    }
}