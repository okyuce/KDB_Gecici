using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.Extensions;
using FluentValidation;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGiris;

public class KullaniciGirisCommandValidator : AbstractValidator<KullaniciGirisCommand>
{
    public KullaniciGirisCommandValidator()
    {
        RuleFor(x => x.KullaniciAdi)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Kullanıcı Adı"));

        RuleFor(x => x.Sifre)
            .Must(FluentValidationExtension.NotWhiteSpace)
            .WithMessage(string.Format(ValidationMessageConstants.BOS_OLAMAZ, "Şifre"));
    }
}