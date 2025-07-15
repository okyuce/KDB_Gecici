using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.SifremiUnuttumKullanici;

public class SifremiUnuttumKullaniciCommandValidator : AbstractValidator<SifremiUnuttumKullaniciCommand>
{

    public SifremiUnuttumKullaniciCommandValidator()
    {
        RuleFor(x => x.KullaniciAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
                .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Kullanıcı Adı"))
            .Length(3, 75)
                .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ + " En az 3 en fazla 75 karakter olmalıdır!", "Kullanıcı Adı"));

        RuleFor(x => x.Eposta)
            .Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "E-Posta"));

        RuleFor(x => x.TcKimlikNo).Must(FluentValidationExtension.NotEmpty)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "TC Kimlik Numarası"));

        RuleFor(x => x.TcKimlikNo)
            .Must(x => FluentValidationExtension.TcDogrula(x.ToString()))
            .WithMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "TC Kimlik Numarası"))
            .When(x => FluentValidationExtension.NotEmpty(x.TcKimlikNo));

       
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